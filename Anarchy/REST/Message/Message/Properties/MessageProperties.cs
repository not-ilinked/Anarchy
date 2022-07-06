using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Discord
{
    [MultipartFormDataProvider]
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


        [JsonProperty("components")]
        public List<MessageComponent> Components { get; set; }


        [JsonProperty("attachments")]
        public List<PartialDiscordAttachment> Attachments { get; set; }


        public bool ShouldSerializeAttachments()
        {
            return Attachments != null;
        }

        public bool ShouldSerializeReplyTo()
        {
            return ReplyTo != null;
        }

        public bool ShouldSerializeEmbed()
        {
            return Embed != null;
        }

        public bool ShouldSerializeComponents()
        {
            return Components != null;
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (ShouldSerializeAttachments())
                for (byte i = 0; i < Attachments.Count; ++i) Attachments[i].Id = i;
        }
    }
}
