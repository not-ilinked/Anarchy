

using System.Text.Json.Serialization;

namespace Discord
{
    public class RolePositionUpdate
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("position")]
        public int Position { get; set; }
    }
}
