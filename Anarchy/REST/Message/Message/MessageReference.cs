using Newtonsoft.Json;

namespace Discord
{
    public class MessageReference
    {
        public MessageReference() { }

        public MessageReference(ulong messageId)
        {
            MessageId = messageId;
        }

        public MessageReference(ulong guildId, ulong messageId) : this(messageId)
        {
            GuildId = guildId;
        }

        [JsonProperty("channel_id")]
        public ulong ChannelId { get; internal set; }

        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }

        [JsonProperty("message_id")]
        public ulong MessageId { get; private set; }
    }
}
