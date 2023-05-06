

using System.Text.Json.Serialization;
namespace Discord.Gateway
{
    public class ActivityProperties
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public ActivityType Type { get; set; }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
