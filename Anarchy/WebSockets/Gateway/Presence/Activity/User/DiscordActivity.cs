using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class DiscordActivity : Controllable
    {
        [JsonProperty("type")]
        public ActivityType Type { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
