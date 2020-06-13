using Newtonsoft.Json;

namespace Discord
{
    public class MessageReactionUpdate : Controllable
    {
        public MessageReactionUpdate()
        {
            OnClientUpdated += (sender, e) => Emoji.SetClient(Client);
        }


        [JsonProperty("emoji")]
        public PartialEmoji Emoji { get; private set; }


        [JsonProperty("message_id")]
        public ulong MessageId { get; private set; }


        [JsonProperty("channel_id")]
        public ulong ChannelId { get; private set; }


        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }


        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }


        public override string ToString()
        {
            return Emoji.ToString();
        }
    }
}
