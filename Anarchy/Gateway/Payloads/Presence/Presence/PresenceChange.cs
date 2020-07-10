using Newtonsoft.Json;
using System;

namespace Discord.Gateway
{
    public class PresenceChange
    {
        [JsonProperty("status")]
        private string _status;
        [JsonIgnore]
        public UserStatus Status
        {
            get
            {
                return UserStatusConverter.FromString(_status);
            }
            set { _status = UserStatusConverter.ToString(value); }
        }


        [JsonProperty("game")]
        public Activity Activity { get; set; }


        [JsonProperty("since")]
#pragma warning disable CS0169, IDE0051
        private readonly long _since;
#pragma warning restore CS0169, IDE0051


        [JsonProperty("afk")]
        private readonly bool _afk = true;


        public override string ToString()
        {
            return Status.ToString();
        }
    }
}
