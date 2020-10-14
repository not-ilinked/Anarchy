using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using YoutubeExplode;

namespace MusicBot
{
    class Program
    {
        public static YoutubeClient YouTubeClient { get; private set; } = new YoutubeClient();
        public static Dictionary<ulong, MusicPlayer> Players { get; private set; } = new Dictionary<ulong, MusicPlayer>();
        public static Color EmbedColor = Color.FromArgb(114, 137, 218);

        static void Main(string[] args)
        {
            DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                Intents = DiscordGatewayIntent.Guilds | DiscordGatewayIntent.GuildMessages | DiscordGatewayIntent.GuildVoiceStates
            });
            client.OnLoggedIn += Client_OnLoggedIn;
            client.CreateCommandHandler(";");

            Console.Write("Token: ");
            client.Login(Console.ReadLine());

            Thread.Sleep(-1);
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine("Logged in");

            client.SetActivity(new StreamActivityProperties() 
            { 
                Name = "Powered by anarchy", 
                Url = "https://twitch.tv/ilinked" 
            });
        }
    }
}
