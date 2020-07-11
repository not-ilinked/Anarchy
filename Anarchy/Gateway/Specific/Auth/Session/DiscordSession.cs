using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Discord.Gateway
{
    public class DiscordSession
    {
        [JsonProperty("status")]
        public UserStatus Status { get; private set; }

        [JsonProperty("session_id")]
        public string SessionId { get; private set; }


        [JsonProperty("client_info")]
        public DiscordSessionClientInfo ClientInfo { get; private set; }


        [JsonProperty("activities")]
        public List<Activity> Activities { get; private set; }
    }
}
