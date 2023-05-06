

using System.Text.Json.Serialization;

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

        [JsonPropertyName("channel_id")]
        public ulong ChannelId { get; internal set; }

        [JsonPropertyName("guild_id")]
        public ulong GuildId { get; private set; }

        [JsonPropertyName("message_id")]
        public ulong MessageId { get; private set; }
    }
}
