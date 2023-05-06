using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Discord
{
    public class MessageProperties
    {
        public MessageProperties()
        {
            _nonce = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("nonce")]
#pragma warning disable IDE0052
        private readonly string _nonce;
#pragma warning restore

        [JsonPropertyName("tts")]
        public bool Tts { get; set; }

        [JsonPropertyName("message_reference")]
        public MessageReference ReplyTo { get; set; }

        [JsonPropertyName("embed")]
        public DiscordEmbed Embed { get; set; }

        [JsonPropertyName("components")]
        public List<MessageComponent> Components { get; set; }

        [JsonPropertyName("attachments")]
        public List<PartialDiscordAttachment> Attachments { get; set; }

        public bool ShouldSerializeAttachments()
        {
            return Attachments != null && Attachments.Count > 0;
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

        internal IEnumerable<(string FileName, DiscordAttachmentFile File, int Id)> GetAttachmentFiles()
        {
            return ShouldSerializeAttachments()
                ? Attachments.Select((a, index) => (a.FileName, a.File, index))
                : null;
        }
    }
}
