using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class UserListeningActivity : UserActivity
    {
        [JsonProperty("state")]
        public string Authors { get; private set; }


        [JsonProperty("details")]
        public string Song { get; private set; }


        public override string ToString()
        {
            return Song;
        }
    }
}
