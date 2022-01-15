using Newtonsoft.Json;

namespace Discord
{
    public class WelcomeChannel : Controllable
    {
        [JsonProperty("channel_id")]
        private readonly ulong _id;

        public MinimalTextChannel Channel => new MinimalTextChannel(_id).SetClient(Client);

        [JsonProperty("description")]
        public string Description { get; private set; }


        [JsonProperty("emoji_id")]
        public ulong? EmojiId { get; private set; }


        [JsonProperty("emoji_name")]
        public string EmojiName { get; private set; }
    }
}
