

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class DiscordActivity : Controllable
    {
        [JsonPropertyName("type")]
        public ActivityType Type { get; private set; }

        [JsonPropertyName("id")]
        public string Id { get; private set; }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
