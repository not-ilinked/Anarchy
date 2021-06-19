using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    internal class Container
    {
        [JsonProperty("_errors")]
        public IReadOnlyList<DiscordFieldError> Fields { get; private set; }
    }
}
