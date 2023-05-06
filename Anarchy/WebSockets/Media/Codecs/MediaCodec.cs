using System;
using System.Text.Json.Serialization;

namespace Discord.Media
{
    internal class MediaCodec
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        private string _type;

        public CodecType Type
        {
            get
            {
                return (CodecType) Enum.Parse(typeof(CodecType), _type, true);
            }
            set
            {
                _type = value.ToString().ToLower();
            }
        }

        [JsonPropertyName("priority")]
        public int Priority { get; set; } = 1000;

        [JsonPropertyName("payload_type")]
        public byte PayloadType { get; set; }
    }
}
