using System.Collections.Generic;
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


        public static async Task<GuildVerificationForm> GetGuildVerificationFormAsync(this DiscordClient client, ulong guildId, string inviteCode)
        {
            return (await client.HttpClient.GetAsync($"/guilds/{guildId}/member-verification?with_guild=false&invite_code={inviteCode}"))
                                    .Deserialize<GuildVerificationForm>();
        }

        public static GuildVerificationForm GetGuildVerificationForm(this DiscordClient client, ulong guildId, string inviteCode)
        {
            return client.GetGuildVerificationFormAsync(guildId, inviteCode).GetAwaiter().GetResult();
        }


        public static async Task<GuildVerificationForm> ModifyGuildVerificationFormAsync(this DiscordClient client, ulong guildId, VerificationFormProperties properties)
        {
            return (await client.HttpClient.PatchAsync($"/guilds/{guildId}/member-verification", properties)).Deserialize<GuildVerificationForm>();
        }

        public static GuildVerificationForm ModifyGuildVerificationForm(this DiscordClient client, ulong guildId, VerificationFormProperties properties)
        {
            return client.ModifyGuildVerificationFormAsync(guildId, properties).GetAwaiter().GetResult();
        }

        public static async Task<VerificationFormResponse> SubmitGuildVerificationFormAsync(this DiscordClient client, ulong guildId, string formVersion, List<GuildVerificationFormField> fields)
        {
            return (await client.HttpClient.PutAsync($"/guilds/{guildId}/requests/@me", new { version = formVersion, form_fields = fields }))
                                    .Deserialize<VerificationFormResponse>().SetClient(client);
        }

        public static VerificationFormResponse SubmitGuildVerificationForm(this DiscordClient client, ulong guildId, string formVersion, List<GuildVerificationFormField> fields)
        {
            return client.SubmitGuildVerificationFormAsync(guildId, formVersion, fields).GetAwaiter().GetResult();
        }
    }
}
