using System.Threading.Tasks;

namespace Discord
{
    public static class CommunityExtensions
    {
        public static async Task<WelcomeScreen> GetWelcomeScreenAsync(this DiscordClient client, ulong guildId)
        {
            return (await client.HttpClient.GetAsync($"/guilds/{guildId}/welcome-screen"))
                                    .Deserialize<WelcomeScreen>().SetClient(client);
        }

        public static WelcomeScreen GetWelcomeScreen(this DiscordClient client, ulong guildId)
        {
            return client.GetWelcomeScreenAsync(guildId).GetAwaiter().GetResult();
        }


        public static async Task<WelcomeScreen> ModifyWelcomeScreenAsync(this DiscordClient client, ulong guildId, WelcomeScreenProperties properties)
        {
            return (await client.HttpClient.PatchAsync($"/guilds/{guildId}/welcome-screen", properties))
                                    .Deserialize<WelcomeScreen>().SetClient(client);
        }

        public static WelcomeScreen ModifyWelcomeScreen(this DiscordClient client, ulong guildId, WelcomeScreenProperties properties)
        {
            return client.ModifyWelcomeScreenAsync(guildId, properties).GetAwaiter().GetResult();
        }
    }
}
