using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Discord
{
    public class DiscordTemplateGuild : ControllableEx
    {
        public DiscordTemplateGuild()
        {
            OnClientUpdated += (sender, e) =>
            {
                Roles.SetClientsInList(Client);
                Channels.SetClientsInList(Client);
            };
            JsonUpdated += (sender, json) =>
            {
                Channels = json.Value<JArray>("channels").DeserializeWithJson<GuildChannel>();
            };
        }

        [JsonProperty("region")]
        public string Region { get; private set; }


        [JsonProperty("verification_level")]
        public GuildVerificationLevel VerificationLevel { get; private set; }


        [JsonProperty("explicit_content_filter")]
        public ExplicitContentFilter ExplicitContentFilter { get; private set; }


        [JsonProperty("roles")]
        public IReadOnlyList<DiscordRole> Roles { get; private set; }


        [JsonProperty("channels")]
        public IReadOnlyList<GuildChannel> Channels { get; private set; }


        internal void SetGuildId(ulong guildId)
        {
            foreach (var role in Roles)
                role.GuildId = guildId;

            foreach (var channel in Channels)
                channel.GuildId = guildId;
        }
    }
}
