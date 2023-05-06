using System;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class GameActivityProperties : ActivityProperties
    {
        public GameActivityProperties()
        {
            _timestamps = new TimestampProperties();
        }

        [JsonPropertyName("type")]
        public new ActivityType Type
        {
            get { return ActivityType.Game; }
        }

        [JsonPropertyName("details")]
        public string Details { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("timestamps")]
        private readonly TimestampProperties _timestamps;

        public TimeSpan Elapsed
        {
            get { return _timestamps.Start; }
            set { _timestamps.Start = value; }
        }
    }
}
