using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    public static class GuildTemplateExtensions
    {
        public static async Task<IReadOnlyList<DiscordGuild>> CreateTemplatedGuildAsync(this DiscordClient client, string templateCode, string name, DiscordImage icon = null)
        {
            return (await client.HttpClient.PostAsync("/guilds/templates/" + templateCode, new GuildCreationProperties()
            {
                Name = name,
                Icon = icon
            })).Deserialize<IReadOnlyList<DiscordGuild>>().SetClientsInList(client);
        }

        /// <summary>
        /// Creates a guild from a template
        /// </summary>
        public static IReadOnlyList<DiscordGuild> CreateTemplatedGuild(this DiscordClient client, string templateCode, string name, DiscordImage icon = null)
        {
            return client.CreateTemplatedGuildAsync(templateCode, name, icon).GetAwaiter().GetResult();
        }

        public static async Task<DiscordGuildTemplate> CreateGuildTemplateAsync(this DiscordClient client, ulong guildId, string name, string description)
        {
            return (await client.HttpClient.PostAsync($"/guilds/{guildId}/templates", $"{{\"name\":\"{name}\",\"description\":\"{description}\"}}")).Deserialize<DiscordGuildTemplate>().SetClient(client);
        }

        /// <summary>
        /// Creates a guild template
        /// </summary>
        public static DiscordGuildTemplate CreateGuildTemplate(this DiscordClient client, ulong guildId, string name, string description)
        {
            return client.CreateGuildTemplateAsync(guildId, name, description).GetAwaiter().GetResult();
        }

        public static async Task<DiscordGuildTemplate> DeleteGuildTemplateAsync(this DiscordClient client, ulong guildId, string templateCode)
        {
            return (await client.HttpClient.DeleteAsync($"/guilds/{guildId}/templates/{templateCode}")).Deserialize<DiscordGuildTemplate>().SetClient(client);
        }

        /// <summary>
        /// Deletes a guild template
        /// </summary>
        public static DiscordGuildTemplate DeleteGuildTemplate(this DiscordClient client, ulong guildId, string templateCode)
        {
            return client.DeleteGuildTemplateAsync(guildId, templateCode).GetAwaiter().GetResult();
        }

        public static async Task<IReadOnlyList<DiscordGuildTemplate>> GetGuildTemplatesAsync(this DiscordClient client, ulong guildId)
        {
            return (await client.HttpClient.GetAsync($"/guilds/{guildId}/templates")).Deserialize<List<DiscordGuildTemplate>>().SetClientsInList(client);
        }

        /// <summary>
        /// Gets templates from a guild
        /// </summary>
        public static IReadOnlyList<DiscordGuildTemplate> GetGuildTemplates(this DiscordClient client, ulong guildId)
        {
            return client.GetGuildTemplatesAsync(guildId).GetAwaiter().GetResult();
        }

        public static async Task<DiscordGuildTemplate> GetGuildTemplateAsync(this DiscordClient client, string code)
        {
            return (await client.HttpClient.GetAsync("/guilds/templates/" + code)).Deserialize<DiscordGuildTemplate>().SetClient(client);
        }

        public static DiscordGuildTemplate GetGuildTemplate(this DiscordClient client, string code)
        {
            return client.GetGuildTemplateAsync(code).GetAwaiter().GetResult();
        }
    }
}
