namespace Discord
{
    using Newtonsoft.Json;

    public class PartialDiscordAttachment
    {
        [JsonProperty("id")]
        internal ulong Id { get; set; }


        [JsonProperty("filename")]
        public string FileName { get; set; }


        [JsonProperty("description")]
        public string Description { get; set; }


        [JsonIgnore]
        public DiscordImage Image { get; set; }
    }
}
