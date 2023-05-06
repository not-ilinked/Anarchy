

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class PresenceProperties
    {
        private readonly DiscordParameter<UserStatus> _statusParam = new DiscordParameter<UserStatus>();
        [JsonPropertyName("status")]
        public UserStatus Status
        {
            get { return _statusParam; }
            set { _statusParam.Value = value; }
        }

        private readonly DiscordParameter<ActivityProperties> _activityParam = new DiscordParameter<ActivityProperties>();
        [JsonPropertyName("game")]
        public ActivityProperties Activity
        {
            get { return _activityParam; }
            set { _activityParam.Value = value; }
        }

        [JsonPropertyName("since")]
#pragma warning disable CS0169, IDE0051
        private readonly long _since;
#pragma warning restore CS0169, IDE0051

        [JsonPropertyName("afk")]
        private readonly bool _afk = true;

        public override string ToString()
        {
            return Status.ToString();
        }
    }
}
