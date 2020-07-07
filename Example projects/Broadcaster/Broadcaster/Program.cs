using Discord;
using Discord.Voice;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Broadcaster
{
    // notes:
    // currently this bot doesn't give a single shit about being banned

    class Program
    {
        public static ConcurrentQueue<GuildInfo> AvailableGuilds { get; private set; } = new ConcurrentQueue<GuildInfo>();
        public static ConcurrentQueue<BroadcastClient> AvailableClients { get; private set; } = new ConcurrentQueue<BroadcastClient>();
        public static byte[] Audio { get; private set; }
        public static string[] Nicknames { get; private set; }

        static void Main(string[] args)
        {
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));

            Audio = DiscordVoiceUtils.ReadFromFile(config.AudioPath);
            Nicknames = config.Nicknames;

            foreach (var inv in File.ReadAllLines(config.InvitesPath))
                AvailableGuilds.Enqueue(new GuildInfo(inv));

            StartAssignerAsync();

            foreach (var token in File.ReadAllLines(config.TokensPath))
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
