using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class DiscordSession
    {
        [JsonProperty("status")]
        public UserStatus Status { get; private set; }

        [JsonProperty("session_id")]
        public string Id { get; private set; }

        [JsonProperty("client_info")]
        public DiscordSessionClientInfo ClientInfo { get; private set; }

        [JsonProperty("activities")]
        public IReadOnlyList<DiscordActivity> Activities { get; private set; }
    }
}
