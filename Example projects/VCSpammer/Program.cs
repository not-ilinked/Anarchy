using Discord;
using Discord.Gateway;
using Discord.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VCSpammer
{
    class Program
    {
        public static ulong TargetGuildId { get; private set; }
        public static string AudioPath { get; private set; }

        public static object ChannelLookupLock = new object();
        public static List<ulong> BotAccounts = new List<ulong>();
        public static List<ulong> BottedChannels = new List<ulong>();

        public static readonly int MinimumHumanParticipants = 1;

        static void Main(string[] args)
        {
            Console.Write("Guild ID: ");
            TargetGuildId = ulong.Parse(Console.ReadLine());

            Console.Write("Audio file path: ");
            AudioPath = Console.ReadLine();

            string[] tokens = File.ReadAllLines("Tokens.txt");

            Console.WriteLine($"Logging into {tokens.Length} accounts");

            foreach (var token in tokens)
            {
                var client = new DiscordSocketClient(new DiscordSocketConfig() { VoiceChannelConnectTimeout = 5000 });
                client.OnLoggedIn += Client_OnLoggedIn;
                client.Login(token);
            }

            Thread.Sleep(-1);
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            BotAccounts.Add(client.User.Id);

            Console.WriteLine("Logged into " + client.User.ToString());

            Connect(client);
        }

        private static void Session_OnDisconnected(DiscordVoiceSession session, DiscordMediaCloseEventArgs args)
        {
            BottedChannels.Remove(session.Channel.Id);
            Connect(session.Client);
        }

        private static void Session_OnConnected(DiscordVoiceSession session, EventArgs e)
        {
            Console.WriteLine(session.Client.User.ToString() + " has joined " + session.Channel.Id);

            var stream = session.CreateStream(((VoiceChannel)session.Client.GetChannel(session.Channel.Id)).Bitrate);

            session.SetSpeakingState(DiscordSpeakingFlags.Soundshare);

            CancellationTokenSource source = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (true)
                {
                    if (GetParticipantCount(session.Client, session.Channel.Id) < MinimumHumanParticipants)
                    {
                        source.Cancel();
                        Console.WriteLine(session.Client.User.ToString() + " is switching channel, due to there being noone in the current one");
                        BottedChannels.Remove(session.Channel.Id);
                        session.SetChannel(FindAvailableChannel(session.Client));

                        return;
                    }

                    Thread.Sleep(100);
                }
            });

            while (session.State == MediaSessionState.Authenticated && !source.IsCancellationRequested)
                stream.CopyFrom(DiscordVoiceUtils.GetAudioStream(AudioPath), source.Token);
        }

        private static void Connect(DiscordSocketClient client)
        {
            var session = client.JoinVoiceChannel(new VoiceStateProperties() { ChannelId = FindAvailableChannel(client), Muted = true });
            session.OnConnected += Session_OnConnected;
            session.OnDisconnected += Session_OnDisconnected;
            session.Connect();
        }

        private static int GetParticipantCount(DiscordSocketClient client, ulong channelId) =>
            client.GetChannelVoiceStates(channelId).Where(s => !BotAccounts.Contains(s.UserId)).Count();

        private static ulong FindAvailableChannel(DiscordSocketClient client)
        {
            while (true)
            {
                var guild = client.GetCachedGuild(TargetGuildId);

                foreach (var ch in guild.Channels.Where(c => c.Type == ChannelType.Voice).OrderBy(c => GetParticipantCount(client, c.Id)).Reverse())
                {
                    var voiceChannel = (VoiceChannel)ch;

                    var perms = guild.ClientMember.GetPermissions(voiceChannel.PermissionOverwrites);

                    if (perms.Has(DiscordPermission.ViewChannel) && perms.Has(DiscordPermission.ConnectToVC) && perms.Has(DiscordPermission.SpeakInVC))
                    {
                        int voiceStates = GetParticipantCount(client, voiceChannel.Id);
                        if (voiceStates >= MinimumHumanParticipants && (voiceChannel.UserLimit == 0 || voiceStates < voiceChannel.UserLimit))
                        {
                            lock (ChannelLookupLock)
                            {
                                if (!BottedChannels.Contains(voiceChannel.Id))
                                {
                                    BottedChannels.Add(voiceChannel.Id);

                                    Console.WriteLine(client.User.ToString() + " has found the channel " + voiceChannel.Id);
                                    return voiceChannel.Id;
                                }
                            } 
                        }
                    }
                }

                Thread.Sleep(100);
            }
        }
    }
}
