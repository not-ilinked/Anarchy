

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class UnavailableGuild
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("unavailable")]
        public bool Unavailable { get; private set; }

        public bool Removed
        {
            get { return !Unavailable; }
        }
    }
}
