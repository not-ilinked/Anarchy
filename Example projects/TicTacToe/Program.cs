using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TicTacToe
{
    internal class Program
    {
        public static Dictionary<string, Game> Games = new Dictionary<string, Game>();

        private static void Main(string[] args)
        {
            Console.Write("Bot token: ");
            string token = Console.ReadLine();

            // this bot does not actually require any intents, but we have to add at least one :/
            DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig() { Intents = DiscordGatewayIntent.Guilds });
            client.OnLoggedIn += Client_OnLoggedIn;
            client.Login("Bot " + token);

            Thread.Sleep(-1);
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine("Logged in");

            client.RegisterSlashCommands();
        }
    }
}
