using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord.Media
{
    internal class GoLiveUpdate
    {
        [JsonProperty("stream_key")]
        public string StreamKey { get; private set; }

        [JsonProperty("region")]
        public string Region { get; private set; }

        [JsonProperty("paused")]
        public bool Paused { get; private set; }

        [JsonProperty("viewer_ids")]
        public IReadOnlyList<ulong> ViewerIds { get; private set; }
    }
}
