

using System.Text.Json.Serialization;

namespace Discord
{
    public class GuildMuteConfig
    {
        [JsonPropertyName("end_time")]
        public int? EndTime { get; set; }

        [JsonPropertyName("selected_time_window")]
        public int SelectedTimeWindow { get; set; }
    }
}
