using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class PresenceProperties
    {
        private readonly DiscordParameter<UserStatus> _statusParam = new DiscordParameter<UserStatus>();
        [JsonProperty("status")]
        public UserStatus Status
        {
            get => _statusParam;
            set => _statusParam.Value = value;
        }


        private readonly DiscordParameter<ActivityProperties> _activityParam = new DiscordParameter<ActivityProperties>();
        [JsonProperty("game")]
        public ActivityProperties Activity
        {
            get => _activityParam;
            set => _activityParam.Value = value;
        }


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
