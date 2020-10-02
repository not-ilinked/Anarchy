using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class UserGameActivity : UserActivity
    {
        [JsonProperty("application_id")]
        public string ApplicationId { get; private set; }
    }
}
