using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Discord.Gateway
{
    public class DiscordSession
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
            private set { _status = UserStatusConverter.ToString(value); }
        }

        [JsonProperty("session_id")]
        public string SessionId { get; private set; }


        [JsonProperty("client_info")]
        public DiscordSessionClientInfo ClientInfo { get; private set; }


        [JsonProperty("activities")]
        public List<Activity> Activities { get; private set; }
    }
}
