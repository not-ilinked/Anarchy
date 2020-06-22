using System.Collections.Generic;
using System.Drawing;

namespace Discord
{
    public static class GuildTemplateExtensions
    {
        /// <summary>
        /// Creates a guild from a template
        /// </summary>
        public static IReadOnlyList<DiscordGuild> CreateTemplatedGuild(this DiscordClient client, string templateCode, string name, Image icon = null)
        {
            GuildCreationProperties properties = new GuildCreationProperties()
            {
                Name = name,
                Icon = icon
            };

            return client.HttpClient.Post("/guilds/templates/" + templateCode, properties).Deserialize<IReadOnlyList<DiscordGuild>>().SetClientsInList(client);
        }


        /// <summary>
        /// Creates a guild template
        /// </summary>
        public static DiscordGuildTemplate CreateGuildTemplate(this DiscordClient client, ulong guildId, string name, string description)
        {
            return client.HttpClient.Post($"/guilds/{guildId}/templates", $"{{\"name\":\"{name}\",\"description\":\"{description}\"}}").DeserializeEx<DiscordGuildTemplate>().SetClient(client);
        }


        /// <summary>
        /// Deletes a guild template
        /// </summary>
        public static DiscordGuildTemplate DeleteGuildTemplate(this DiscordClient client, ulong guildId, string templateCode)
        {
            return client.HttpClient.Delete($"/guilds/{guildId}/templates/{templateCode}").DeserializeEx<DiscordGuildTemplate>().SetClient(client);
        }


        /// <summary>
        /// Gets templates from a guild
        /// </summary>
        public static IReadOnlyList<DiscordGuildTemplate> GetGuildTemplates(this DiscordClient client, ulong guildId)
        {
            return client.HttpClient.Get($"/guilds/{guildId}/templates").DeserializeExArray<DiscordGuildTemplate>().SetClientsInList(client);
        }


        public static DiscordGuildTemplate GetGuildTemplate(this DiscordClient client, string code)
        {
            return client.HttpClient.Get("/guilds/templates/" + code).DeserializeEx<DiscordGuildTemplate>().SetClient(client);
        }
    }
}
