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
                var dupedEmojis = new Dictionary<ulong, ulong>();
                foreach (var emoji in targetGuild.Emojis)
                {
                    try
                    {
                        var created = guild.CreateEmoji(new EmojiProperties() { Name = emoji.Name, Image = emoji.Icon.Download() });
                        dupedEmojis[emoji.Id.Value] = created.Id.Value;
                    }
                    catch (DiscordHttpException ex)
                    {
                        if (ex.Code == DiscordError.InvalidFormBody || ex.Code == DiscordError.MaximumEmojis)
                            break;
                        else if (ex.Code != DiscordError.InvalidFormBody)
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
                Dictionary<ulong, ulong> dupedChannels = new Dictionary<ulong, ulong>();
                foreach (var channel in targetGuild.Channels.OrderBy(c => c.Type != ChannelType.Category))
                {
                    var ourChannel = guild.CreateChannel(channel.Name, TranslateChannelType(channel), channel.ParentId.HasValue ? dupedChannels[channel.ParentId.Value] : (ulong?)null);
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

                    dupedChannels[channel.Id] = ourChannel.Id;
                }

                bool hasWelcomeScreen = TryGetWelcomeScreen(targetGuild, out var welcomeScreen);
                bool hasVerificationForm = TryGetVerificationForm(targetGuild, out var verificationForm);

                if (hasWelcomeScreen || hasVerificationForm)
                {
                    Console.WriteLine("Enabling Community...");

                    guild.Modify(new GuildProperties()
                    {
                        DefaultNotifications = GuildDefaultNotifications.OnlyMentions,
                        VerificationLevel = GuildVerificationLevel.Low,
                        Features = new List<string>() { "COMMUNITY", "WELCOME_SCREEN_ENABLED" },
                        PublicUpdatesChannelId = dupedChannels[targetGuild.PublicUpdatesChannel.Id],
                        RulesChannelId = dupedChannels[targetGuild.RulesChannel.Id],
                        Description = targetGuild.Description,
                        ExplicitContentFilter = ExplicitContentFilter.KeepMeSafe
                    });

                    if (hasWelcomeScreen)
                    {
                        List<WelcomeChannelProperties> channels = new List<WelcomeChannelProperties>();

                        foreach (var channel in welcomeScreen.Channels)
                        {
                            ulong? emojiId = null;

                            if (channel.EmojiId.HasValue)
                            {
                                if (dupedEmojis.TryGetValue(channel.EmojiId.Value, out ulong emId)) emojiId = emId;
                                else continue; // this relates to discord's ratelimit issue
                            }

                            channels.Add(new WelcomeChannelProperties()
                            {
                                ChannelId = dupedChannels[channel.Channel.Id],
                                Description = channel.Description,
                                EmojiId = emojiId,
                                EmojiName = channel.EmojiName
                            });
                        }

                        guild.ModifyWelcomeScreen(new WelcomeScreenProperties() { Enabled = true, Description = welcomeScreen.Description, Channels = channels });
                    }

                    if (hasVerificationForm)
                    {
                        // this doesnt work for some reason
                        guild.ModifyVerificationForm(new VerificationFormProperties()
                        {
                            Description = verificationForm.Description,
                            Enabled = true,
                            Fields = verificationForm.Fields.ToList()
                        });
                    }

                    Console.WriteLine("Updating news channels...");
                    foreach (var channel in targetGuild.Channels)
                    {
                        if (channel.Type == ChannelType.News)
                            client.ModifyGuildChannel(dupedChannels[channel.Id], new TextChannelProperties() { News = true });
                    }
                }

                Console.WriteLine("Done!");
            }
            catch (DiscordHttpException ex)
            {
                if (ex.Code == DiscordError.UnknownGuild) Console.WriteLine("You are not in this guild");
                else Console.WriteLine(ex.ToString());
            }
        }

        private static bool TryGetWelcomeScreen(SocketGuild targetGuild, out WelcomeScreen screen)
        {
            try
            {
                screen = targetGuild.GetWelcomeScreen();
                return true;
            }
            catch (DiscordHttpException)
            {
                screen = null;
                return false;
            }
        }

        private static bool TryGetVerificationForm(SocketGuild targetGuild, out GuildVerificationForm form)
        {
            try
            {
                if (TryGetInvite(targetGuild, out string code))
                {
                    form = targetGuild.GetVerificationForm(code);
                    return true;
                }
            }
            catch (DiscordHttpException) { }

            form = null;
            return false;
        }

        private static bool TryGetInvite(SocketGuild guild, out string code)
        {
            if (guild.VanityInvite != null)
            {
                code = guild.VanityInvite;
                return true;
            }
            else
            {
                foreach (var channel in guild.Channels)
                {
                    if (channel.IsText && guild.ClientMember.GetPermissions(channel.PermissionOverwrites).Has(DiscordPermission.CreateInstantInvite))
                    {
                        code = ((TextChannel)channel).CreateInvite().Code;
                        return true;
                    }
                }
            }

            code = null;
            return false;
        }
    }
}
