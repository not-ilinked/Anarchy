using Newtonsoft.Json;

namespace Broadcaster
{
    public class Config
    {
        [JsonProperty("audio")]
        public string AudioPath { get; private set; }


        [JsonProperty("nicknames")]
        public string[] Nicknames { get; private set; }


        [JsonProperty("invites")]
        public string InvitesPath { get; private set; }


        [JsonProperty("tokens")]
        public string TokensPath { get; private set; }
    }
}
