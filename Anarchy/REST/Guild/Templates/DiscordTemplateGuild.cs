using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        [JsonProperty("region")]
        public string Region { get; private set; }


        [JsonProperty("verification_level")]
        public GuildVerificationLevel VerificationLevel { get; private set; }


        [JsonProperty("explicit_content_filter")]
        public ExplicitContentFilter ExplicitContentFilter { get; private set; }


        [JsonProperty("roles")]
        public IReadOnlyList<DiscordRole> Roles { get; private set; }


        [JsonProperty("channels")]
        [JsonConverter(typeof(DeepJsonConverter<GuildChannel>))]
        private List<GuildChannel> _channels;

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
