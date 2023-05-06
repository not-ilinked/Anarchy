using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class GuildUnreadMessages : Controllable
    {
        public GuildUnreadMessages()
        {
            OnClientUpdated += (s, args) => Channels.SetClientsInList(Client);
        }

        [JsonPropertyName("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild
        {
            get { return new MinimalGuild(_guildId).SetClient(Client); }
        }

        [JsonPropertyName("channel_unread_updates")]
        public IReadOnlyList<ChannelUnreadMessages> Channels { get; private set; }
    }
}
