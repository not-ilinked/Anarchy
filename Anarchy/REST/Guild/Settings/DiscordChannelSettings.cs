using Newtonsoft.Json;

namespace Discord
{
    public class DiscordChannelSettings : Controllable
    {
        [JsonProperty("muted")]
        public bool Muted { get; private set; }
    }
}
