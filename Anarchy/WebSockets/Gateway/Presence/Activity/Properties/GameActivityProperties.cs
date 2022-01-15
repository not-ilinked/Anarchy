using Newtonsoft.Json;
using System;

namespace Discord.Gateway
{
    public class GameActivityProperties : ActivityProperties
    {
        public GameActivityProperties()
        {
            _timestamps = new TimestampProperties();
        }


        [JsonProperty("type")]
        public new ActivityType Type => ActivityType.Game;


        [JsonProperty("details")]
        public string Details { get; set; }


        [JsonProperty("state")]
        public string State { get; set; }


        [JsonProperty("timestamps")]
        private readonly TimestampProperties _timestamps;

        public TimeSpan Elapsed
        {
            get => _timestamps.Start;
            set => _timestamps.Start = value;
        }
    }
}
