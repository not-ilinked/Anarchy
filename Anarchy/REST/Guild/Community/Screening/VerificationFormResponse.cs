using Newtonsoft.Json;
using System;

namespace Discord
{
    public class VerificationFormResponse : Controllable
    {
        [JsonProperty("created_at")]
        public DateTime SubmittedAt { get; private set; }


        [JsonProperty("rejection_reason")]
        public string RejectionReason { get; private set; }


        [JsonProperty("application_status")]
        public string ApplicationStatus { get; private set; }

        public bool Approved => ApplicationStatus == "APPROVED";


        [JsonProperty("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild => new MinimalGuild(_guildId).SetClient(Client);
    }
}
