using Newtonsoft.Json;

namespace Discord
{
    class RateLimit
    {
        [JsonProperty("global")]
        public bool Global { get; private set; }


        [JsonProperty("message")]
        public string Message { get; private set; }


        [JsonProperty("retry_after")]
        public int RetryAfter { get; private set; }


        public override string ToString()
        {
            return RetryAfter.ToString();
        }


        public static implicit operator int(RateLimit instance)
        {
            return instance.RetryAfter;
        }
    }
}