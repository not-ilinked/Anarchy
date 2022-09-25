using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordStageInstance : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        [JsonProperty("guild_id")]
        private readonly ulong _guildId;
        public MinimalGuild Guild => new MinimalGuild(_guildId).SetClient(Client);

        [JsonProperty("channel_id")]
        private readonly ulong _channelId;
        public MinimalChannel Channel => new MinimalChannel(_channelId).SetClient(Client);

        [JsonProperty("topic")]
        public string Topic { get; private set; }

        [JsonProperty("discoverable_disabled")]
        public bool DiscoveryDisabled { get; private set; }

        [JsonProperty("privacy_level")]
        public StagePrivacyLevel PrivacyLevel { get; private set; }

        [JsonProperty("invite_code")]
        public string InviteCode { get; private set; }

        public Task SetClientSpeakingAsync(bool speaker) => Client.SetClientStageSpeakingAsync(_guildId, _channelId, speaker);
        public void SetClientSpeaking(bool speaker) => SetClientSpeakingAsync(speaker).GetAwaiter().GetResult();

        public Task DeleteAsync() => Client.DeleteStageInstanceAsync(_channelId);
        public void Delete() => DeleteAsync().GetAwaiter().GetResult();
    }
}
