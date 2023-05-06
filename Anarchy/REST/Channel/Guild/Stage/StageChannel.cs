using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discord
{
    public class StageChannel : VoiceChannel
    {
        [JsonPropertyName("topic")]
        public string Topic { get; private set; }

        [JsonPropertyName("nsfw")]
        public string Nsfw { get; private set; }

        public Task<DiscordStageInstance> CreateInstanceAsync(string topic, StagePrivacyLevel privacyLevel = StagePrivacyLevel.GuildOnly) =>
            Client.CreateStageInstanceAsync(Id, topic, privacyLevel);

        public DiscordStageInstance CreateInstance(string topic, StagePrivacyLevel privacyLevel = StagePrivacyLevel.GuildOnly) =>
            CreateInstanceAsync(topic, privacyLevel).GetAwaiter().GetResult();

    }
}
