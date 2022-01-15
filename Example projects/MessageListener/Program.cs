using Discord;
using Discord.Gateway;
using System;
using System.Threading;

namespace MessageLogger
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DiscordSocketClient client = new DiscordSocketClient();
            client.OnLoggedIn += OnLoggedIn;
            client.OnMessageReceived += OnMessageReceived;

            Console.Write("Token: ");
            client.Login(Console.ReadLine());

            Thread.Sleep(-1);
        }


        private static void OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine($"Logged into {args.User}");
        }


        private static void OnMessageReceived(DiscordSocketClient client, MessageEventArgs args)
        {
            string from;

            DiscordChannel channel = client.GetChannel(args.Message.Channel.Id);

            if (channel.InGuild)
            {
                from = $"#{channel.Name} / {client.GetCachedGuild(((TextChannel)channel).Guild.Id).Name}";
            }
            else
            {
                PrivateChannel privChannel = (PrivateChannel)channel;
                from = privChannel.Name ?? privChannel.Recipients[0].ToString();
            }

            Console.WriteLine($"[{from}] {args.Message.Author}: {args.Message.Content}");
        }
    }
}
