using Newtonsoft.Json;

namespace Discord
{
    public class RolePositionUpdate
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }


        [JsonProperty("position")]
        public int Position { get; set; }
    }
}
