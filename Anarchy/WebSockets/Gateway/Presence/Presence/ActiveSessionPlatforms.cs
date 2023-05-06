

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class ActiveSessionPlatforms
    {
        [JsonPropertyName("desktop")]
        public UserStatus Desktop { get; private set; } = UserStatus.Offline;

        [JsonPropertyName("web")]
        public UserStatus Website { get; private set; } = UserStatus.Offline;

        [JsonPropertyName("mobile")]
        public UserStatus Mobile { get; private set; } = UserStatus.Offline;
    }
}
