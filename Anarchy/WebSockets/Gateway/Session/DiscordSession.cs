using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class DiscordSession
    {
        [JsonPropertyName("status")]
        public UserStatus Status { get; private set; }

        [JsonPropertyName("session_id")]
        public string Id { get; private set; }

        [JsonPropertyName("client_info")]
        public DiscordSessionClientInfo ClientInfo { get; private set; }

        [JsonPropertyName("activities")]
        public IReadOnlyList<DiscordActivity> Activities { get; private set; }
    }
}
