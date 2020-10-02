using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class WelcomeScreen : Controllable
    {
        public WelcomeScreen()
        {
            OnClientUpdated += (s, e) =>
            {
                Channels.SetClientsInList(Client);
            };
        }

        [JsonProperty("description")]
        public string Description { get; private set; }


        [JsonProperty("welcome_channels")]
        public IReadOnlyList<WelcomeChannel> Channels { get; private set; }
    }
}
