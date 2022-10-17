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
        private readonly ulong _channelId;

        public MinimalTextChannel Channel
        {
            get
            {
                return new MinimalTextChannel(_channelId).SetClient(Client);
            }
        }

        [JsonProperty("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(_guildId).SetClient(Client);
            }
        }

        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }

        public override string ToString()
        {
            return Emoji.ToString();
        }
    }
}
