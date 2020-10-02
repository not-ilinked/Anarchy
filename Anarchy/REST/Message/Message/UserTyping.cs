using System;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class UserTyping : Controllable
    {
        [JsonProperty("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel
        {
            get
            {
                return new MinimalTextChannel(_channelId).SetClient(Client);
            }
        }


        [JsonProperty("guild_id")]
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


        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }


        [JsonProperty("timestamp")]
#pragma warning disable CS0649
        private readonly ulong _timestamp;
#pragma warning restore CS0649

        [JsonIgnore]
        public DateTimeOffset Timestamp
        {
            get { return DateTimeOffset.FromUnixTimeSeconds((long)_timestamp); }
        }


        public override string ToString()
        {
            return Timestamp.ToString();
        }
    }
}
