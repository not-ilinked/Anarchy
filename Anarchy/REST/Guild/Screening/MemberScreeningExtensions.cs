using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public static class MemberScreeningExtensions
    {
        public static async Task<GuildVerificationForm> GetGuildVerificationFormAsync(this DiscordClient client, ulong guildId, string inviteCode)
        {
            return (await client.HttpClient.GetAsync($"/guilds/{guildId}/member-verification?with_guild=false&invite_code={inviteCode}"))
                                    .Deserialize<GuildVerificationForm>();
        }

        public static GuildVerificationForm GetGuildVerificationForm(this DiscordClient client, ulong guildId, string inviteCode)
        {
            return client.GetGuildVerificationFormAsync(guildId, inviteCode).GetAwaiter().GetResult();
        }


        public static async Task<VerificationFormResponse> SubmitGuildVerificationFormAsync(this DiscordClient client, ulong guildId, string formVersion, List<GuildVerificationFormField> fields)
        {
            foreach (var field in fields)
            {
                if (field.Required && field.Response == null)
                    throw new MissingFieldResponseException(field.Label);
            }

            return (await client.HttpClient.PutAsync($"/guilds/{guildId}/requests/@me", new VerificationFormSubmissionProperties() { Version = formVersion, Fields = fields }))
                                    .Deserialize<VerificationFormResponse>().SetClient(client);
        }

        public static VerificationFormResponse SubmitGuildVerificationForm(this DiscordClient client, ulong guildId, string formVersion, List<GuildVerificationFormField> fields)
        {
            return client.SubmitGuildVerificationFormAsync(guildId, formVersion, fields).GetAwaiter().GetResult();
        }
    }
}
