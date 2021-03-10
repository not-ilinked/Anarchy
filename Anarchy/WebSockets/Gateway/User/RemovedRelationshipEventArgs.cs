using Newtonsoft.Json;
using System;

namespace Discord
{
    public class RemovedRelationshipEventArgs : EventArgs
    {
        [JsonProperty("id")]
        public ulong UserId { get; private set; }


        [JsonProperty("type")]
        public RelationshipType PreviousType { get; private set; }
    }
}
