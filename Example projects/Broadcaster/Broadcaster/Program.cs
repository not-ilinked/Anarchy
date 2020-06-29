using Discord;
using Discord.Voice;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Broadcaster
{
    // notes:
    // should probably allow the user to specify a file or something
    // currently this bot doesn't give a single shit about being banned

    class Program
    {
        public static ConcurrentQueue<GuildInfo> AvailableGuilds { get; private set; } = new ConcurrentQueue<GuildInfo>();
        public static ConcurrentQueue<BroadcastClient> AvailableClients { get; private set; } = new ConcurrentQueue<BroadcastClient>();
        public static byte[] Audio = DiscordVoiceUtils.ReadFromFile(@"C:\Users\edhbu\Downloads\goodbye_monkey.mp4");

        static void Main(string[] args)
        {
            foreach (var inv in File.ReadAllLines("Invites.txt"))
                AvailableGuilds.Enqueue(new GuildInfo(inv));

            StartAssignerAsync();

            foreach (var token in File.ReadAllLines("Tokens.txt"))
            {
                try
                {
                    new BroadcastClient(token);
                }
                catch (InvalidTokenException ex) 
                {
                    Console.WriteLine($"{ex.Token} is invalid!");
                }
            }

            Thread.Sleep(-1);
        }


        private static async void StartAssignerAsync()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (AvailableGuilds.TryDequeue(out GuildInfo guild))
                    {
                        BroadcastClient client;

                        while (!AvailableClients.TryDequeue(out client)) { Thread.Sleep(100); }

                        client.BroadcastToAsync(guild);
                    }
                    else
                        Thread.Sleep(100);
                }
            });
        }
    }
}
