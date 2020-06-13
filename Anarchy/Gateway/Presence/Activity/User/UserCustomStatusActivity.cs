using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class UserCustomStatusActivity : UserActivity
    {
        [JsonProperty("state")]
        public string Text { get; private set; }


        [JsonProperty("emoji")]
        public PartialEmoji Emoji { get; private set; }


        public override string ToString()
        {
            return Text;
        }
    }
}
