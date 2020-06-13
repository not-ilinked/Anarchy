using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for sending a message
    /// </summary>
    internal class MessageProperties
    {
        public MessageProperties()
        {
            _nonce = "";
        }


        [JsonProperty("content")]
        public string Content { get; set; }


        [JsonProperty("nonce")]
#pragma warning disable CS0414, IDE0052
        private readonly string _nonce;
#pragma warning restore CS0414, IDE0052


        [JsonProperty("tts")]
        public bool Tts { get; set; }


        [JsonProperty("embed")]
        public DiscordEmbed Embed { get; set; }


        public bool ShouldSerializeEmbed()
        {
            return Embed != null;
        }
    }
}
