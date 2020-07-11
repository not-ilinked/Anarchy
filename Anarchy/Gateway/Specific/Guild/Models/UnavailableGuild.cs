using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class UnavailableGuild
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("unavailable")]
        public bool Unavailable { get; private set; }


        public bool Removed
        {
            get { return !Unavailable; }
        }
    }
}
