using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class MemberListUpdateOperation : Controllable
    {
        public MemberListUpdateOperation()
        {
            OnClientUpdated += (s, e) => Items.SetClientsInList(Client);
        }

        [JsonPropertyName("range")]
        public int[] Range { get; private set; }

        [JsonPropertyName("index")]
        public int Index { get; private set; }

        [JsonPropertyName("op")]
        public string Type { get; private set; }

        [JsonPropertyName("items")]
        public IReadOnlyList<MemberListItem> Items { get; private set; }
    }
}
