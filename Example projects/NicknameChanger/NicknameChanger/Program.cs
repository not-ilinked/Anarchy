using System;
using System.Threading;
using Discord;

namespace NicknameChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            //create client
            Console.Write("Token: ");
            DiscordClient client = new DiscordClient(Console.ReadLine());

            //get guild
            Console.Write("Guild id: ");
            DiscordGuild guild = client.GetGuild(ulong.Parse(Console.ReadLine()));

            Console.Write("Full nickname: ");
            string nickname = Console.ReadLine();

            Console.WriteLine($"Changing nickname in {guild.Name}...");

            //every time it runs it's gonna add another character to the name, until it's finished where it gets reset
            string currentNick = "";
            while (true)
            {
                for (int i = 0; i < nickname.Length; i++)
                {
                    currentNick += nickname[i];
                    guild.SetNickname(currentNick);
                    Thread.Sleep(1000);
                }

                currentNick = "";
            }
        }
    }
}
