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
                if (_status == "dnd")
                    return UserStatus.DoNotDisturb;
                else
                    return (UserStatus)Enum.Parse(typeof(UserStatus), _status, true);
            }
            private set { _status = value != UserStatus.DoNotDisturb ? value.ToString().ToLower() : "dnd"; }
        }

        [JsonProperty("session_id")]
        public string SessionId { get; private set; }


        [JsonProperty("client_info")]
        public DiscordSessionClientInfo ClientInfo { get; private set; }


        [JsonProperty("activities")]
        public List<Activity> Activities { get; private set; }
    }
}
