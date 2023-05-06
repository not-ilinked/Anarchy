using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discord
{
    public class DiscordStageInstance : Controllable
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("guild_id")]
        private readonly ulong _guildId;
        public MinimalGuild Guild => new MinimalGuild(_guildId).SetClient(Client);

        [JsonPropertyName("channel_id")]
        private readonly ulong _channelId;
        public MinimalChannel Channel => new MinimalChannel(_channelId).SetClient(Client);

        [JsonPropertyName("topic")]
        public string Topic { get; private set; }

        [JsonPropertyName("discoverable_disabled")]
        public bool DiscoveryDisabled { get; private set; }

        [JsonPropertyName("privacy_level")]
        public StagePrivacyLevel PrivacyLevel { get; private set; }

        [JsonPropertyName("invite_code")]
        public string InviteCode { get; private set; }
        public Task SetClientSpeakingAsync(bool speaker) => Client.SetClientStageSpeakingAsync(_guildId, _channelId, speaker);
        public void SetClientSpeaking(bool speaker) => SetClientSpeakingAsync(speaker).GetAwaiter().GetResult();
        public Task DeleteAsync() => Client.DeleteStageInstanceAsync(_channelId);
        public void Delete() => DeleteAsync().GetAwaiter().GetResult();

    }
}