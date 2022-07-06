using System;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class TimestampProperties
    {
        private readonly DiscordParameter<long> _startParam = new DiscordParameter<long>();
        [JsonProperty("start")]
        private long _startValue
        {
            get { return _startParam; }
            set { _startParam.Value = value; }
        }

        private TimeSpan _start;
        public TimeSpan Start
        {
            get { return _start; }
            set
            {
                _start = new TimeSpan(value.Hours > 14 ? 14 : value.Hours, value.Minutes, 0);
                _startValue = (DateTimeOffset.UtcNow - _start).ToUnixTimeMilliseconds();
            }
        }


        public bool ShouldSerialize_startValue()
        {
            return _startParam.Set;
        }
    }
}
