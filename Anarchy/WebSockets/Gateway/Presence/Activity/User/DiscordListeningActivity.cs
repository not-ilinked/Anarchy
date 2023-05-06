

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class DiscordListeningActivity : DiscordActivity
    {
        [JsonPropertyName("state")]
        public string Authors { get; private set; }

        [JsonPropertyName("details")]
        public string Song { get; private set; }

        public override string ToString()
        {
            return Song;
        }
    }
}
