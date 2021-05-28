using Newtonsoft.Json;
using System;

namespace Discord
{
    public class MessageProperties
    {
        public MessageProperties()
        {
            _nonce = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        }


        [JsonProperty("content")]
        public string Content { get; set; }


        [JsonProperty("nonce")]
#pragma warning disable IDE0052
        private readonly string _nonce;
#pragma warning restore


        [JsonProperty("tts")]
        public bool Tts { get; set; }


        [JsonProperty("message_reference")]
        public MessageReference ReplyTo { get; set; }


        [JsonProperty("embed")]
        public DiscordEmbed Embed { get; set; }


        public bool ShouldSerializeReplyTo()
        {
            return ReplyTo != null;
        }

        public bool ShouldSerializeEmbed()
        {
            return Embed != null;
        }
    }
}
