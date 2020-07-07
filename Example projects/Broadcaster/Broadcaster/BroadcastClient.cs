using Discord;
using Discord.Gateway;
using Discord.Voice;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Broadcaster
{
    public class BroadcastClient
    {
        private DiscordSocketClient _client;
        private GuildInfo _currentGuild;

        public BroadcastClient(string token)
        {
            _client = new DiscordSocketClient();
            _client.OnLoggedIn += OnLoggedIn;
            _client.OnJoinedGuild += OnJoinedGuild;
            _client.OnLeftGuild += OnLeftGuild;
            _client.Login(token);
        }

        private void OnLeftGuild(DiscordSocketClient client, GuildUnavailableEventArgs args)
        {
            Console.WriteLine($"[{client.User}] Left guild {args.Guild.Id}.");

            Program.AvailableGuilds.Enqueue(_currentGuild);
        }

        private void OnJoinedGuild(DiscordSocketClient client, SocketGuildEventArgs args)
        {
            if (args.Guild.Id == _currentGuild.Id)
            {
                Console.WriteLine($"[{client.User}] Joined guild {args.Guild.Id}. Processing...");

                BeginBroadcast(args.Guild);
            }
        }

        private void OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine($"[{client.User}] Logged in.");

            StartNicknamerAsync();

            Program.AvailableClients.Enqueue(this);
        }

        public async void BroadcastToAsync(GuildInfo guild)
        {
            await Task.Run(() =>
            {
                _currentGuild = guild;

                try
                {
                    SocketGuild socketGuild = _client.GetCachedGuild(guild.Id);

                    Console.WriteLine($"[{_client.User}] Already in guild {guild.Id}. Processing...");

                    BeginBroadcast(socketGuild);
                }
                catch (DiscordHttpException ex)
                {
                    if (ex.Code == DiscordError.UnknownGuild)
                        _client.JoinGuild(guild.Invite);
                    else
                        throw;
                }
            });
        }

        private void BeginBroadcast(SocketGuild guild)
        {
            DiscordPermission permissions = guild.GetMember(_client.User.Id).GetPermissions();

            // if our role(s) don't allow connecting + speaking in vc, those permissions being explicitly marked as "allowed" in perm overwrites is necessary
            OverwrittenPermissionState minimumOverwriteState = permissions.HasFlag(DiscordPermission.ConnectToVC) && permissions.HasFlag(DiscordPermission.SpeakInVC) ? OverwrittenPermissionState.Inherit : OverwrittenPermissionState.Allow;

            do
            {
                foreach (var channel in guild.Channels.Where(c => c.Type == ChannelType.Voice))
                {
                    var ourRoles = guild.GetMember(_client.User.Id).Roles;

                    bool connect = true;
                    foreach (var overwrite in channel.PermissionOverwrites)
                    {
                        if (overwrite.Type == PermissionOverwriteType.Role && ourRoles.Contains(overwrite.AffectedId) || overwrite.Type == PermissionOverwriteType.Member && overwrite.AffectedId == _client.User.Id)
                        {
                            if (overwrite.GetPermissionState(DiscordPermission.ConnectToVC) < minimumOverwriteState || overwrite.GetPermissionState(DiscordPermission.SpeakInVC) < minimumOverwriteState)
                            {
                                connect = false;

                                break;
                            }
                        }
                    }

                    // disabled for now because:
                    // if a the client will move onto an available guild if it's needed
                    // issue is we don't know if that guild has any people connected to the vc either
                    /*
                    if (guild.VoiceStates.Where(v => v.Channel != null && v.Channel.Id == channel.Id && v.UserId != _client.User.Id).Count() == 0)
                        connect = false;
                    */

                    if (connect)
                        Speak(channel.ToVoiceChannel());

                    Thread.Sleep(1000);
                }
            }
            while (Program.AvailableGuilds.IsEmpty || !Program.AvailableClients.IsEmpty);

            Console.WriteLine($"[{_client.User}] Done processing guild.");

            Program.AvailableGuilds.Enqueue(_currentGuild);
            Program.AvailableClients.Enqueue(this);
        }


        private void Speak(VoiceChannel channel)
        {
            bool done = false;

            var session = _client.JoinVoiceChannel(channel.Guild.Id, channel.Id);
            session.OnConnected += (s, args) =>
            {
                Console.WriteLine($"[{_client.User}] Speaking in channel.");

                session.SetSpeakingState(DiscordVoiceSpeakingState.Microphone);

                var stream = session.CreateStream(channel.Bitrate);
                stream.CopyFrom(Program.Audio);

                session.SetSpeakingState(DiscordVoiceSpeakingState.NotSpeaking);

                done = true;
            };

            while (!done) { Thread.Sleep(10); }
        }


        private async void StartNicknamerAsync()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        foreach (var nick in Program.Nicknames)
                        {
                            _client.ChangeClientNickname(_currentGuild.Id, nick);

                            Thread.Sleep(1000);
                        }
                    }
                    catch (DiscordHttpException ex)
                    {
                        if (ex.Code == DiscordError.MissingPermissions || ex.Code == DiscordError.UnknownMember)
                        {
                            ulong expectedGuildId = _currentGuild.Id;

                            while (expectedGuildId == _currentGuild.Id) { Thread.Sleep(100); }
                        }
                        else
                            throw;
                    }
                }
            });
        }
    }
}
