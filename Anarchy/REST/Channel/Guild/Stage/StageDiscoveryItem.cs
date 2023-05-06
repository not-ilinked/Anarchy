using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        [JsonPropertyName("instance")]
        public DiscordStageInstance Instance { get; private set; }

        [JsonPropertyName("speakers")]
        public IReadOnlyList<ulong> Speakers { get; private set; }

        [JsonPropertyName("sample_speaker_members")]
        public IReadOnlyList<GuildMember> SampleSpeakers { get; private set; }

        [JsonPropertyName("participant_count")]
        public uint ParticipantCount { get; private set; }

        [JsonPropertyName("guild")]
        public DiscordGuild Guild { get; private set; }

        [JsonPropertyName("channel")]
        public StageChannel Channel { get; private set; }
    }
}
