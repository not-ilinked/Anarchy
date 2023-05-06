

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class InviteDeletedEventArgs : Controllable
    {
        [JsonPropertyName("code")]
        public string Code { get; private set; }

        [JsonPropertyName("guild_id")]
        private readonly ulong? _guildId;

        public MinimalGuild Guild
        {
            get
            {
                if (_guildId.HasValue)
                    return new MinimalGuild(_guildId.Value).SetClient(Client);
                else
                    return null;
            }
        }

        [JsonPropertyName("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel
        {
            get { return new MinimalTextChannel(_channelId).SetClient(Client); }
        }
    }
}
