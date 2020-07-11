using Newtonsoft.Json;
using System;

namespace Discord.Gateway
{
    public class GameActivity : Activity
    {
        public GameActivity()
        {
            _timestamps = new ActivityTimestamps();
        }


        [JsonProperty("type")]
        public new ActivityType Type
        {
            get { return ActivityType.Game; }
        }


        [JsonProperty("details")]
        public string Details { get; set; }


        [JsonProperty("state")]
        public string State { get; set; }


        [JsonProperty("timestamps")]
        private readonly ActivityTimestamps _timestamps;
        public TimeSpan Elapsed
        {
            get { return _timestamps.Start; }
            set { _timestamps.Start = value; }
        }
    }
}
