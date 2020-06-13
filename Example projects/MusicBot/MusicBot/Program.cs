using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Discord;
using Discord.Gateway;

namespace MusicBot
{
    class Program
    {
        public static Dictionary<ulong, MusicSession> Sessions { get; private set; }
        public static readonly Color EmbedColor = Color.FromArgb(105, 125, 202);
        public static readonly EmbedFooter EmbedFooter = new EmbedFooter()
        {
            Text = "Powered by Anarchy",
            IconUrl = "https://cdn.discordapp.com/attachments/698872354599600220/698934060491210802/Anarchy.png"
        };

        static void Main(string[] args)
        {
            Sessions = new Dictionary<ulong, MusicSession>();

            Console.Write("Token: ");
            string token = Console.ReadLine();

            DiscordSocketClient client = new DiscordSocketClient();
            client.CreateCommandHandler("m;");
            client.OnLoggedIn += Client_OnLoggedIn;
            client.Login(token);

            Thread.Sleep(-1);
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine("Logged in");

            client.SetActivity(new StreamActivity() { Name = "powered by Anarchy", Url = "https://www.twitch.tv/ilinked" });
        }
    }
}
