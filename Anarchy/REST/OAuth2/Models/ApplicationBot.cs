using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Discord
{
    public class ApplicationBot : Controllable
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("username")]
        public string Username { get; private set; }

        [JsonPropertyName("discriminator")]
        public uint Discriminator { get; private set; }

        [JsonPropertyName("avatar")]
        public string AvatarId { get; private set; }

        [JsonPropertyName("token")]
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
