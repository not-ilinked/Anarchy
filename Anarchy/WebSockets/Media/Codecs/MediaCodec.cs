using Newtonsoft.Json;
using System;

namespace Discord.Media
{
    internal class MediaCodec
    {
        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("type")]
        private string _type;

        public CodecType Type
        {
            get => (CodecType)Enum.Parse(typeof(CodecType), _type, true);
            set => _type = value.ToString().ToLower();
        }


        [JsonProperty("priority")]
        public int Priority { get; set; } = 1000;


        [JsonProperty("payload_type")]
        public byte PayloadType { get; set; }
    }
}
