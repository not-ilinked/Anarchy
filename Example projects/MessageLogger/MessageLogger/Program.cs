using System;
using System.Threading;
using Discord;
using Discord.Gateway;

namespace MessageLogger
{
    class Program
    {
        static void Main(string[] args)
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

            if (channel.Type == ChannelType.Text)
                from = $"#{channel.Name} / {client.GetCachedGuild(((TextChannel)channel).Guild.Id).Name}";
            else if (channel.Type == ChannelType.Group)
                from = channel.Name;
            else
                from = ((PrivateChannel)channel).Recipients[0].Username;

            Console.WriteLine($"[{from}] {args.Message.Author}: {args.Message.Content}");
        }
    }
}
