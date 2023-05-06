

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class MemberListGroup
    {
        [JsonPropertyName("id")]
        public string Id { get; private set; }

        [JsonPropertyName("count")]
        public int Count { get; private set; }
    }
}
