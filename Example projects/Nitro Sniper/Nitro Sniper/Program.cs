using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Nitro_Sniper
{
    class Program
    {
        static void Main(string[] args)
        {
            //wrappers in 2020 doe
            Console.WriteLine("Authorization");
            string token = Console.ReadLine();

            DiscordSocketClient client = new DiscordSocketClient();

            client.OnMessageReceived += Client_OnMessageReceived;

            client.Login(token);

            Thread.Sleep(-1);
        }
        static List<string> CodeCache = new List<string>();
        private static void Client_OnMessageReceived(DiscordSocketClient client, Discord.MessageEventArgs args)
        {
            foreach (Match m in Regex.Matches(args.Message.Content, @"(discord.com\/gifts\/|discordapp.com\/gifts\/|discord.gift\/)([a-zA-Z0-9]+)", RegexOptions.Multiline))
            {
                string NitroCode = m.Value.Split('/')[m.Value.Split('/').Length - 1];
                if (CodeCache.Contains(NitroCode)) continue;
                CodeCache.Add(NitroCode);
                try
                {
                    //cause doesnt return any data?
                    client.HttpClient.Post($"https://discordapp.com/api/v6/entitlements/gift-codes/{NitroCode}/redeem");
                    Console.WriteLine("Claimed nitro");
                }
                catch { }
            }
        }
    }
}
