using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord
{
    public class ApplicationBot : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        [JsonProperty("username")]
        public string Username { get; private set; }

        [JsonProperty("discriminator")]
        public uint Discriminator { get; private set; }

        [JsonProperty("avatar")]
        public string AvatarId { get; private set; }

        [JsonProperty("token")]
        public string Token { get; private set; }

        public async Task AuthorizeAsync(ulong guildId, DiscordPermission permissions, string captchaKey)
        {
            await Client.AuthorizeBotAsync(Id, guildId, permissions, captchaKey);
        }

        public void Authorize(ulong guildId, DiscordPermission permissions, string captchaKey)
        {
            AuthorizeAsync(guildId, permissions, captchaKey).GetAwaiter().GetResult();
        }

        public override string ToString()
        {
            return $"{Username}#{Discriminator}";
        }

        public static implicit operator ulong(ApplicationBot instance)
        {
            return instance.Id;
        }
    }
}
