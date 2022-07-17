using System.Text.Json;
using Discord.Gateway;

namespace Discord
{
    [TestClass]
    public static class Globals
    {
        internal static AppSettings Settings { get; private set; } =
            JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(@"./appsettings.json"))!;

        public static class FileNames
        {
            public const string Image1 = @".\Resources\image1.png";
            public const string Image2 = @".\Resources\image2.jpg";
            public const string PoetryTxt = @".\Resources\poetry.txt";
        }

        public static DiscordSocketClient? Client { get; set; }

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            var autoResetEvent = new AutoResetEvent(false);

            var client = new DiscordSocketClient();
            client.OnLoggedIn += OnLoggedIn;
            client.Login(Settings.Token);

            void OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
            {
                Console.WriteLine($"Logged into {args.User}");
                autoResetEvent.Set();
            }

            autoResetEvent.WaitOne();

            Client = client;
        }
    }
}