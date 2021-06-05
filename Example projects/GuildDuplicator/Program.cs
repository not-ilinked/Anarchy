using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Discord;
using Discord.Gateway;

namespace GuildDuplicator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Token: ");
            string token = Console.ReadLine();

            DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig() { RetryOnRateLimit = false });
            client.OnLoggedIn += Client_OnLoggedIn;
            client.Login(token);

            Thread.Sleep(-1);
        }

        private static ChannelType TranslateChannelType(GuildChannel channel)
        {
            if (channel.IsText) return ChannelType.Text;
            else if (channel.IsVoice) return ChannelType.Voice;
            else return ChannelType.Category;
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.Write("Guild ID: ");
            ulong guildId = ulong.Parse(Console.ReadLine());

            try
            {
                var targetGuild = client.GetCachedGuild(guildId);

                Console.WriteLine("Creating guild...");
                var guild = client.CreateGuild(targetGuild.Name, targetGuild.Icon == null ? null : targetGuild.Icon.Download(), targetGuild.Region);

                Console.WriteLine("Creating roles...");
                Dictionary<ulong, ulong> dupedRoles = new Dictionary<ulong, ulong>();
                foreach (var role in targetGuild.Roles.OrderBy(r => r.Position).Reverse())
                {
                    var properties = new RoleProperties() { Name = role.Name, Color = role.Color, Mentionable = role.Mentionable, Permissions = role.Permissions, Seperated = role.Seperated };

                    if (role.Name == "@everyone") dupedRoles[role.Id] = guild.EveryoneRole.Modify(properties).Id;
                    else dupedRoles[role.Id] = guild.CreateRole(new RoleProperties() { Name = role.Name, Color = role.Color, Mentionable = role.Mentionable, Permissions = role.Permissions, Seperated = role.Seperated }).Id;
                }

                Console.WriteLine("Creating emojis...");
                foreach (var emoji in targetGuild.Emojis)
                {
                    try
                    {
                        guild.CreateEmoji(new EmojiProperties() { Name = emoji.Name, Image = emoji.Icon.Download() });
                    }
                    catch (DiscordHttpException ex)
                    {
                        if (ex.Code == DiscordError.InvalidFormBody) // This is used whenever discord wants to give us parameter-specific errors
                        {
                            if (ex.InvalidParameters.TryGetValue("image", out var errors))
                            {
                                if (!errors.ContainsKey("BINARY_TYPE_MAX_SIZE"))
                                    foreach (var error in errors) Console.WriteLine($"Error in {error.Key}. Reason: {error.Value}");
                            }
                        }
                        else if (ex.Code == DiscordError.MaximumEmojis)
                            break;
                        else
                            throw;
                    }
                    catch (RateLimitException)
                    {
                        break; // Discord likes to throw this after us after duping most of a guild's emojis, and it's so big that we might as well stop trying.
                    }
                }

                Console.WriteLine("Removing default channels...");
                foreach (var channel in guild.GetChannels())
                    channel.Delete();

                Console.WriteLine("Creating channels...");
                Dictionary<ulong, ulong> dupedCategories = new Dictionary<ulong, ulong>();
                foreach (var channel in targetGuild.Channels.OrderBy(c => c.Type != ChannelType.Category))
                {
                    var ourChannel = guild.CreateChannel(channel.Name, TranslateChannelType(channel), channel.ParentId.HasValue ? dupedCategories[channel.ParentId.Value] : (ulong?)null);
                    ourChannel.Modify(new GuildChannelProperties() { Position = channel.Position });

                    if (ourChannel.Type == ChannelType.Text)
                    {
                        var channelAsText = (TextChannel)channel;
                        ((TextChannel)ourChannel).Modify(new TextChannelProperties() { Nsfw = channelAsText.Nsfw, SlowMode = channelAsText.SlowMode, Topic = channelAsText.Topic });
                    }
                    else if (ourChannel.Type == ChannelType.Voice)
                    {
                        var channelAsVoice = (VoiceChannel)channel;
                        ((VoiceChannel)ourChannel).Modify(new VoiceChannelProperties() { Bitrate = Math.Min(96000, channelAsVoice.Bitrate), UserLimit = Math.Min(99, channelAsVoice.UserLimit) });
                    }

                    foreach (var overwrite in channel.PermissionOverwrites)
                    {
                        if (overwrite.Type == PermissionOverwriteType.Role)
                            ourChannel.AddPermissionOverwrite(dupedRoles[overwrite.AffectedId], PermissionOverwriteType.Role, overwrite.Allow, overwrite.Deny);
                    }

                    if (ourChannel.Type == ChannelType.Category)
                        dupedCategories[channel.Id] = ourChannel.Id;
                }
            }
            catch (DiscordHttpException ex)
            {
                if (ex.Code == DiscordError.UnknownGuild) Console.WriteLine("You are not in this guild");
                else Console.WriteLine(ex.ToString());
            }
        }
    }
}
