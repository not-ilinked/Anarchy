using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord
{
    internal class RecipientList
    {
        [JsonProperty("recipients")]
        public IReadOnlyList<ulong> Recipients { get; set; }
    }
}
