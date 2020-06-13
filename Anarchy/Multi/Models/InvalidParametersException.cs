using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Discord
{
    public class InvalidParametersException : DiscordException
    {
        public string ErrorJson { get; private set; }
        public JObject ErrorJsonObject { get; private set; }

        internal InvalidParametersException(DiscordClient client, string errorJson) : base(client, errorJson)
        {
            ErrorJson = errorJson;
            ErrorJsonObject = JsonConvert.DeserializeObject<JObject>(ErrorJson);
        }


        public override string ToString()
        {
            return ErrorJson;
        }
    }
}
