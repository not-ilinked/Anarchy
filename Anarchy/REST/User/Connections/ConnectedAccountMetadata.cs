using Newtonsoft.Json;


namespace Discord
{
    public class ConnectedAccountMetadata
    {
        // neither of these properties would get converted properly so therefore i'm setting them as a string for now.
        [JsonProperty("verified")]
        public string Verified { get; protected set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; protected set; }
    }
}
