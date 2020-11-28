using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Discord
{
    internal class DiscordHttpError
    {
        [JsonProperty("code")]
        public DiscordError Code { get; private set; }


        [JsonProperty("errors")]
        public Dictionary<string, InvalidParameter.Container> Fields { get; private set; }
        

        [JsonProperty("message")]
        public string Message { get; private set; }


        public DiscordHttpError()
        { }


        public DiscordHttpError(DiscordError code, string message)
        {
            Code = code;
            Fields = new Dictionary<string, InvalidParameter.Container>();
            Message = message;
        }
    }
}
