using System;
using System.Text.Json.Serialization;

namespace Discord
{
    public class VerificationFormResponse : Controllable
    {
        [JsonPropertyName("created_at")]
        public DateTime SubmittedAt { get; private set; }

        [JsonPropertyName("rejection_reason")]
        public string RejectionReason { get; private set; }

        [JsonPropertyName("application_status")]
        public string ApplicationStatus { get; private set; }

        public bool Approved => ApplicationStatus == "APPROVED";

        [JsonPropertyName("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild => new MinimalGuild(_guildId).SetClient(Client);
    }
}
