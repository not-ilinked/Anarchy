using Discord;
using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AntiRaid
{
    class Program
    {
        public static Dictionary<ulong, List<DiscordMessage>> Messages = new Dictionary<ulong, List<DiscordMessage>>();
        public static BanQueue BanQueue = new BanQueue();

        public static readonly int MaxMessages = 10;
        public static readonly TimeSpan MessageExpiration = new TimeSpan(0, 0, 10);

        static void Main(string[] args)
        {
            Console.WriteLine("Token: ");
            string token = Console.ReadLine();

            BanQueue.Start();

            DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig() { Intents = GatewayIntentBundles.Guilds | GatewayIntentBundles.GuildMessages | GatewayIntentBundles.GuildAdministration });
            client.OnLoggedIn += Client_OnLoggedIn;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnJoinedGuild += (s, e) => Messages[e.Guild.Id] = new List<DiscordMessage>();
            client.Login(token);

            Thread.Sleep(-1);
        }

        private static void Client_OnMessageReceived(DiscordSocketClient client, MessageEventArgs args)
        {
            if (args.Message.Guild != null && !string.IsNullOrEmpty(args.Message.Content))
            {
                var messages = Messages[args.Message.Guild.Id];
                messages.RemoveAll(m => m.SentAt < DateTime.UtcNow - MessageExpiration);

                messages.Add(args.Message);

                if (messages.Count >= MaxMessages && messages.Select(x => x.Content).Distinct().Count() == 1)
                {
                    Console.WriteLine("Raid detected");

                    var msgsCopy = new List<DiscordMessage>(messages);
                    messages.Clear();

                    foreach (var msg in msgsCopy)
                        BanQueue.Enqueue(msg.Author.Member);

                    if (client.User.Type == DiscordUserType.Bot)
                    {
                        foreach (var group in msgsCopy.GroupBy(m => m.Channel.Id))
                            client.DeleteMessages(group.Key, group.Select(m => m.Id).ToList());
                    }
                    else
                    {
                        // we have to go the slow route :(
                        foreach (var msg in msgsCopy)
                            msg.Delete();
                    }
                }
            }
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            if (client.User.Type == DiscordUserType.User)
            {
                foreach (var guild in args.Guilds)
                    Messages[guild.Id] = new List<DiscordMessage>();
            }

            Console.WriteLine("Logged in");
        }
    }
}
