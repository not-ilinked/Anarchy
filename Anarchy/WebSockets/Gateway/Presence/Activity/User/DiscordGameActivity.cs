using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class DiscordGameActivity : DiscordActivity
    {
        [JsonPropertyName("application_id")]
        public string ApplicationId { get; private set; }

        [JsonPropertyName("timestamps")]
        private readonly JsonElement _obj;

        public DateTimeOffset? Since
        {
            get
            {
                if (_obj.ValueKind != JsonValueKind.Null)
                    return DateTimeOffset.FromUnixTimeMilliseconds(_obj.GetProperty("start").GetInt64());
                else
                    return null;
            }
        }
    }
}
