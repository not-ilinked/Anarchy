using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class StageDiscoveryItem : Controllable
    {
        public StageDiscoveryItem()
        {
            OnClientUpdated += (s, e) =>
            {
                Instance.SetClient(Client);
                SampleSpeakers.SetClientsInList(Client);
                Guild.SetClient(Client);
                Channel.SetClient(Client);
            };
        }

        [JsonProperty("instance")]
        public DiscordStageInstance Instance { get; private set; }

        [JsonProperty("speakers")]
        public IReadOnlyList<ulong> Speakers { get; private set; }

        [JsonProperty("sample_speaker_members")]
        public IReadOnlyList<GuildMember> SampleSpeakers { get; private set; }

        [JsonProperty("participant_count")]
        public uint ParticipantCount { get; private set; }

        [JsonProperty("guild")]
        public DiscordGuild Guild { get; private set; }

        [JsonProperty("channel")]
        public StageChannel Channel { get; private set; }
    }
}
