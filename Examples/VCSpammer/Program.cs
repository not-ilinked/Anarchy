using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Gateway;
using Discord.Media;

namespace VCSpammer
{
    internal class Program
    {
        public static GuildInvite Invite { get; private set; }
        public static string AudioPath { get; private set; }

        public static object ChannelLookupLock = new object();
        public static List<DiscordSocketClient> BotAccounts = new List<DiscordSocketClient>();

        private static void Main(string[] args)
        {
            Console.WriteLine("Loading puppeteer...");
            DiscordPuppeteer.Start();

            Console.Write("Guild invite: ");
            Invite = (GuildInvite) new DiscordClient().GetInvite(Console.ReadLine());

            Console.Write("Audio file path: ");
            AudioPath = Console.ReadLine();

            string[] tokens = File.ReadAllLines("Tokens.txt");

            Console.WriteLine($"Logging into {tokens.Length} accounts");

            foreach (var token in tokens)
            {
                var client = new DiscordSocketClient(new DiscordSocketConfig() { VoiceChannelConnectTimeout = 5000 });
                client.OnLoggedIn += Client_OnLoggedIn;
                client.OnJoinedVoiceChannel += Client_OnJoinedVoiceChannel;
                client.OnLeftVoiceChannel += Client_OnLeftVoiceChannel;
                client.OnJoinedGuild += Client_OnJoinedGuild;
                client.Login(token);
            }

            Thread.Sleep(-1);
        }

        private static void Client_OnJoinedGuild(DiscordSocketClient client, SocketGuildEventArgs args)
        {
            if (args.Guild.Id == Invite.Guild.Id)
                WaitConnect(client);
        }

        private static void Client_OnLeftVoiceChannel(DiscordSocketClient client, VoiceDisconnectEventArgs args)
        {
            WaitConnect(client);
        }

        private static void Client_OnJoinedVoiceChannel(DiscordSocketClient client, VoiceConnectEventArgs args)
        {
            Console.WriteLine(client.User.ToString() + " has joined " + args.Client.Channel.Id);

            // exploit that lets you speak while muted (https://www.youtube.com/watch?v=PWzPa_BIv9s)
            args.Client.Microphone.Bitrate = ((VoiceChannel) client.GetChannel(args.Client.Channel.Id)).Bitrate;
            args.Client.Microphone.SetSpeakingState(DiscordSpeakingFlags.Soundshare);

            CancellationTokenSource source = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (true)
                {
                    if (GetParticipantCount(client, args.Client.Channel.Id) == 0)
                    {
                        source.Cancel();
                        if (Connect(client)) Console.WriteLine(client.User.ToString() + " is switching channel, due to there being noone in the current one");
                        else args.Client.Disconnect();

                        return;
                    }

                    Thread.Sleep(100);
                }
            });

            while (args.Client.State == MediaConnectionState.Ready && !source.IsCancellationRequested)
                args.Client.Microphone.CopyFrom(DiscordVoiceUtils.GetAudioStream(AudioPath), source.Token);
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            BotAccounts.Add(client);

            Console.WriteLine("Logged into " + client.User.ToString());

            if (!args.Guilds.Any(g => g.Id == Invite.Guild.Id)) DiscordPuppeteer.JoinGuild(client, Invite.Code);
            else WaitConnect(client);
        }

        private static int GetParticipantCount(DiscordSocketClient client, ulong channelId) =>
            client.GetChannelVoiceStates(channelId).Where(s => !BotAccounts.Any(c => c.User.Id == s.UserId)).Count();

        private static void WaitConnect(DiscordSocketClient client)
        {
            while (!Connect(client))
                Thread.Sleep(100);
        }

        private static bool Connect(DiscordSocketClient client)
        {
            lock (ChannelLookupLock)
            {
                var guild = client.GetCachedGuild(Invite.Guild.Id);

                foreach (var ch in guild.Channels.Where(c => c.Type == ChannelType.Voice && !BotAccounts.Any(a =>
                {
                    var voiceClient = a.GetVoiceClient(Invite.Guild.Id);
                    return voiceClient.Channel != null && voiceClient.Channel.Id == c.Id;
                })).OrderBy(c => GetParticipantCount(client, c.Id)).Reverse())
                {
                    var voiceChannel = (VoiceChannel) ch;

                    var perms = guild.ClientMember.GetPermissions(voiceChannel.PermissionOverwrites);

                    if (perms.Has(DiscordPermission.ViewChannel) && perms.Has(DiscordPermission.ConnectToVC) && perms.Has(DiscordPermission.SpeakInVC))
                    {
                        int voiceStates = GetParticipantCount(client, voiceChannel.Id);
                        if (voiceStates > 0 && (voiceChannel.UserLimit == 0 || voiceStates < voiceChannel.UserLimit))
                        {
                            Console.WriteLine(client.User.ToString() + " has found the channel " + voiceChannel.Id);
                            client.GetVoiceClient(Invite.Guild.Id).Connect(voiceChannel.Id, new VoiceConnectionProperties() { Muted = true });
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
