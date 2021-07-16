using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord.Gateway
{
    public class MemberListUpdateOperation : Controllable
    {
        public MemberListUpdateOperation()
        {
            OnClientUpdated += (s, e) => Items.SetClientsInList(Client);
        }

        [JsonProperty("range")]
        public int[] Range { get; private set; }

        [JsonProperty("index")]
        public int Index { get; private set; }

        [JsonProperty("op")]
        public string Type { get; private set; }

        [JsonProperty("items")]
        public IReadOnlyList<MemberListItem> Items { get; private set; }
    }
}
