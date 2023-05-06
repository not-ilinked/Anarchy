using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordTemplateGuild : Controllable
    {
        public DiscordTemplateGuild()
        {
            OnClientUpdated += (sender, e) =>
            {
                Roles.SetClientsInList(Client);
                Channels.SetClientsInList(Client);
            };
        }

        [JsonPropertyName("region")]
        public string Region { get; private set; }

        [JsonPropertyName("verification_level")]
        public GuildVerificationLevel VerificationLevel { get; private set; }

        [JsonPropertyName("explicit_content_filter")]
        public ExplicitContentFilter ExplicitContentFilter { get; private set; }

        [JsonPropertyName("roles")]
        public IReadOnlyList<DiscordRole> Roles { get; private set; }

        [JsonPropertyName("channels")]
        [JsonConverter(typeof(DeepJsonConverter<GuildChannel>))]
        private readonly List<GuildChannel> _channels;

        public IReadOnlyList<GuildChannel> Channels
        {
            get { return _channels; }
        }

        internal void SetGuildId(ulong guildId)
        {
            foreach (var role in Roles)
                role.GuildId = guildId;

            foreach (var channel in Channels)
                channel.GuildId = guildId;
        }
    }
}
