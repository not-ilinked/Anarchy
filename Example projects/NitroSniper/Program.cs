﻿using Discord;
using Discord.Gateway;
using System;
using System.Text.RegularExpressions;

namespace NitroSniper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            byte[] ok = BitConverter.GetBytes(65535);

            Console.Write("Token: ");
            string token = Console.ReadLine();

            DiscordSocketClient client = new DiscordSocketClient();
            client.OnLoggedIn += Client_OnLoggedIn;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.Login(token);

            Console.ReadLine();
        }

        private static void Client_OnMessageReceived(DiscordSocketClient client, MessageEventArgs args)
        {
            const string giftPrefix = "discord.gift/";

            Match match = Regex.Match(args.Message.Content, giftPrefix + ".{16,24}");

            if (match.Success)
            {
                string code = match.Value.Substring(match.Value.IndexOf(giftPrefix) + giftPrefix.Length);

                try
                {
                    client.RedeemGift(code);

                    Console.WriteLine("Successfully redeemed code " + code);
                }
                catch (DiscordHttpException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine("Logged in");
        }
    }
}
