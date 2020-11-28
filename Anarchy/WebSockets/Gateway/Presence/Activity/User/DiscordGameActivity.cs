using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Discord.Gateway
{
    public class DiscordGameActivity : DiscordActivity
    {
        [JsonProperty("application_id")]
        public string ApplicationId { get; private set; }


        [JsonProperty("timestamps")]
        private readonly JObject _obj;

        public DateTimeOffset? Since
        {
            get
            {
                if (_obj != null)
                    return DateTimeOffset.FromUnixTimeMilliseconds(_obj.Value<long>("start"));
                else
                    return null;
            }
        }
    }
}
