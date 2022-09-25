using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord
{
    public class StageChannel : VoiceChannel
    {
        [JsonProperty("topic")]
        public string Topic { get; private set; }

        [JsonProperty("nsfw")]
        public string Nsfw { get; private set; }

        public Task<DiscordStageInstance> CreateInstanceAsync(string topic, StagePrivacyLevel privacyLevel = StagePrivacyLevel.GuildOnly) =>
            Client.CreateStageInstanceAsync(Id, topic, privacyLevel);

        public DiscordStageInstance CreateInstance(string topic, StagePrivacyLevel privacyLevel = StagePrivacyLevel.GuildOnly) =>
            CreateInstanceAsync(topic, privacyLevel).GetAwaiter().GetResult();

    }
}
