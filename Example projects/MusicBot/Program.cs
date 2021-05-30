using System;
using System.Collections.Generic;
using System.Threading;
using Discord;
using Discord.Gateway;
using Discord.Media;
using YoutubeExplode;

namespace MusicBot
{
    class Program
    {
        public static YoutubeClient YouTubeClient { get; private set; } = new YoutubeClient();

        public static Dictionary<ulong, List<AudioTrack>> TrackLists = new Dictionary<ulong, List<AudioTrack>>();
        public static Dictionary<ulong, DiscordVoiceStream> ActiveSessions = new Dictionary<ulong, DiscordVoiceStream>();

        public static bool CanModifyList(DiscordSocketClient client, DiscordMessage message)
        {
            if (!client.GetVoiceStates(client.User.Id).GuildVoiceStates.TryGetValue(message.Guild.Id, out var ourState) || ourState.Channel == null || !Program.TrackLists.TryGetValue(message.Guild.Id, out var list) || list.Count == 0)
            {
                message.Channel.SendMessage("No song is currently being played");
                return false;
            }
            else if (!client.GetVoiceStates(message.Author.User.Id).GuildVoiceStates.TryGetValue(message.Guild.Id, out var state) || state.Channel == null || state.Channel.Id != ourState.Channel.Id)
            {
                message.Channel.SendMessage("You must be connected to the same voice channel as me to skip songs");
                return false;
            }

            return true;
        }

        static void Main(string[] args)
        {
            Console.Write("Token: ");
            string token = Console.ReadLine();

            DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig() { VoiceChannelConnectTimeout = 5000, Intents = DiscordGatewayIntent.Guilds | DiscordGatewayIntent.GuildMessages | DiscordGatewayIntent.GuildVoiceStates });
            client.CreateCommandHandler(";");
            client.OnLoggedIn += Client_OnLoggedIn;
            client.Login(token);
            
            Thread.Sleep(-1);
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine("Logged in");
            client.SetActivity(new ActivityProperties() { Type = ActivityType.Listening, Name = "music" });
        }
    }
}
