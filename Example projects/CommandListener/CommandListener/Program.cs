using System;
using Discord.Gateway;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace CommandListener
{
    class Program
    {
        static void Main()
        {
            Console.Write("Token: ");
            string token = Console.ReadLine();

            DiscordSocketClient client = new DiscordSocketClient();
            client.OnLoggedIn += Client_OnLoggedIn;
            client.CreateCommandHandler(";");
            client.Login(token);

            Thread.Sleep(-1);
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine("Logged in!");
        }
    }
}
