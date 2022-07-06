using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class GuildUnreadMessages : Controllable
    {
        public GuildUnreadMessages()
        {
            OnClientUpdated += (s, args) => Channels.SetClientsInList(Client);
        }

        [JsonProperty("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild
        {
            get { return new MinimalGuild(_guildId).SetClient(Client); }
        }

        [JsonProperty("channel_unread_updates")]
        public IReadOnlyList<ChannelUnreadMessages> Channels { get; private set; }
    }
}
