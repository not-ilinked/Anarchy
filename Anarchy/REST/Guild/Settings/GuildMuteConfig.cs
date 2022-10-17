using Newtonsoft.Json;

namespace Discord
{
    public class GuildMuteConfig
    {
        [JsonProperty("end_time")]
        public int? EndTime { get; set; }

        [JsonProperty("selected_time_window")]
        public int SelectedTimeWindow { get; set; }
    }
}
