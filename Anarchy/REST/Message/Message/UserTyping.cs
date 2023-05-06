using System;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class UserTyping : Controllable
    {
        [JsonPropertyName("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel
        {
            get
            {
                return new MinimalTextChannel(_channelId).SetClient(Client);
            }
        }

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

        [JsonPropertyName("user_id")]
        public ulong UserId { get; private set; }

        [JsonPropertyName("timestamp")]
#pragma warning disable CS0649
        private readonly ulong _timestamp;
#pragma warning restore CS0649

        [JsonIgnore]
        public DateTimeOffset Timestamp
        {
            get { return DateTimeOffset.FromUnixTimeSeconds((long) _timestamp); }
        }

        public override string ToString()
        {
            return Timestamp.ToString();
        }
    }
}
