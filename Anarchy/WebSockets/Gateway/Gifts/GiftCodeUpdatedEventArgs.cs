
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class GiftCodeUpdatedEventArgs : GiftCodeCreatedEventArgs
    {
        public JsonElement Json { get; internal set; }

        [JsonPropertyName("uses")]
        public uint Uses { get; private set; }

        [JsonPropertyName("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel
        {
            get { return new MinimalTextChannel(_channelId).SetClient(Client); }
        }

        [JsonPropertyName("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild
        {
            get { return new MinimalGuild(_guildId).SetClient(Client); }
        }
    }
}
