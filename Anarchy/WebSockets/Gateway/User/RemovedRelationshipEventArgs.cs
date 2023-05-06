using System;
using System.Text.Json.Serialization;

namespace Discord
{
    public class RemovedRelationshipEventArgs : EventArgs
    {
        [JsonPropertyName("id")]
        public ulong UserId { get; private set; }

        [JsonPropertyName("type")]
        public RelationshipType PreviousType { get; private set; }
    }
}
