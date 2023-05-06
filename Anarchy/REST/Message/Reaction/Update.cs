

using System.Text.Json.Serialization;

namespace Discord
{
    public class MessageReactionUpdate : Controllable
    {
        public MessageReactionUpdate()
        {
            OnClientUpdated += (sender, e) => Emoji.SetClient(Client);
        }

        [JsonPropertyName("emoji")]
        public PartialEmoji Emoji { get; private set; }

        [JsonPropertyName("message_id")]
        public ulong MessageId { get; private set; }

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
        private readonly ulong _guildId;

        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(_guildId).SetClient(Client);
            }
        }

        [JsonPropertyName("user_id")]
        public ulong UserId { get; private set; }

        public override string ToString()
        {
            return Emoji.ToString();
        }
    }
}
