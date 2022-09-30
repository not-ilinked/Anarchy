using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Discord;
using Discord.Gateway;

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
            DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig() {
                Intents = GatewayIntentBundles.Guilds,
                ApiVersion = 10
            });
            client.OnLoggedIn += Client_OnLoggedIn;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.Login("Bot " + token);

            /** Trigger Interaction Examples

            ulong ulChannelID = 936587123811835934;
            ulong ulBotID = 646937666251915264;

            var trigger = new TriggerInteractions(client, ulChannelID, ulBotID);

            trigger.SendSlash("album card add", "Cyan");
            var msgs = client.GetChannelMessages(ulChannelID, 1);

            trigger.ClickButton(msgs[0], "Button");
            trigger.SelectValue(msgs[0], "SelectValue");

            **/

            Thread.Sleep(-1);
        }

        private static void Client_OnMessageReceived(DiscordSocketClient client, MessageEventArgs args)
        {
            if (args.Message.Content == "!ping")
            {
                args.Message.Channel.SendMessage($"🏓 Pong ! Api latency: {client.ping}ms");
            } else if (args.Message.Content == "!clean")
            {
                var listMsg = args.Message.Channel.GetMessages(new MessageFilters { Limit = 10});

                args.Message.Channel.DeleteMessages(listMsg.Select(m => m.Id).ToList());
            }
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine("Logged in");

            try
            {
                client.RegisterSlashCommands(958528245605756978);
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
