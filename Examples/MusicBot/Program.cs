using System;
using System.Collections.Generic;
using System.Threading;
using Discord;
using Discord.Gateway;
using Discord.Media;
using YoutubeExplode;

namespace MusicBot
{
    internal class Program
    {
        public static YoutubeClient YouTubeClient { get; private set; } = new YoutubeClient();

        public static Dictionary<ulong, TrackQueue> TrackLists = new();

        public static bool CanModifyList(DiscordSocketClient client, DiscordMessage message)
        {
            var voiceClient = client.GetVoiceClient(message.Guild.Id);

            if (voiceClient.State != MediaConnectionState.Ready)
                message.Channel.SendMessage("I am not connected to a voice channel");
            else if (!client.GetVoiceStates(message.Author.User.Id).GuildVoiceStates.TryGetValue(message.Guild.Id, out var state) || state.Channel == null || state.Channel.Id != voiceClient.Channel.Id)
                message.Channel.SendMessage("You must be connected to the same voice channel as me to skip songs");
            else if (!TrackLists.TryGetValue(message.Guild.Id, out var queue) || queue.Tracks.Count == 0)
                message.Channel.SendMessage("The queue is empty");
            else return true;

            return false;
        }

        private static void Main()
        {
            Console.Write("Token: ");
            string token = Console.ReadLine();

            var client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                VoiceChannelConnectTimeout = 5000,
                HandleIncomingMediaData = false,
                Intents = DiscordGatewayIntent.Guilds | DiscordGatewayIntent.GuildMessages | DiscordGatewayIntent.GuildVoiceStates
            });

            client.CreateCommandHandler(";");
            client.OnLoggedIn += Client_OnLoggedIn;
            client.OnJoinedVoiceChannel += Client_OnJoinedVoiceChannel;
            client.Login(token);

            Thread.Sleep(-1);
        }

        private static void Client_OnJoinedVoiceChannel(DiscordSocketClient client, VoiceConnectEventArgs args)
        {
            if (TrackLists.TryGetValue(args.Client.Guild.Id, out var list) && !list.Running)
                list.Start();
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine("Logged in");
            client.SetActivity(new ActivityProperties() { Type = ActivityType.Listening, Name = "music" });
        }
    }
}
