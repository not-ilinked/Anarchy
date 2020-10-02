using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    // API v8
    internal class InvalidParameter
    {
        internal class Container
        {
            [JsonProperty("_errors")]
            public IReadOnlyList<InvalidParameter> Fields { get; private set; }
        }

        [JsonProperty("code")]
        public string Code { get; private set; }


        [JsonProperty("message")]
        public string Message { get; private set; }
    }
}
