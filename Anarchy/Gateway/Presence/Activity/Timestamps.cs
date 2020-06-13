using Newtonsoft.Json;
using System;

namespace Discord.Gateway
{
    internal class ActivityTimestamps
    {
        private bool _startSet;
        [JsonProperty("start")]
#pragma warning disable IDE0052
        private long _startValue;
#pragma warning restore IDE0052

        [JsonIgnore]
        private TimeSpan _start;
        [JsonIgnore]
        public TimeSpan Start
        {
            get { return _start; }
            set
            {
                _start = new TimeSpan(value.Hours > 14 ? 14 : value.Hours, value.Minutes, 0);
                DateTime time = DateTime.UtcNow;
                _startValue = new DateTimeOffset(time.Year, 
                                                 time.Month,
                                                 time.Day,
                                                 time.Hour,
                                                 time.Minute,
                                                 time.Second,
                                                 _start)
                                              .ToUnixTimeMilliseconds();

                _startSet = true;
            }
        }


        public bool ShouldSerialize_startValue()
        {
            return _startSet;
        }
    }
}
