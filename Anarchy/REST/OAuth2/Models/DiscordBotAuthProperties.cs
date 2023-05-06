

using System.Text.Json.Serialization;
namespace Discord
{
    internal class DiscordBotAuthProperties
    {
        [JsonPropertyName("authorize")]
#pragma warning disable IDE0051, CS0414
        private readonly bool _auth = true;
#pragma warning restore IDE0051, CS0414

        [JsonPropertyName("bot_guild_id")]
        public ulong GuildId { get; set; }

        [JsonPropertyName("captcha_key")]
        public string CaptchaKey { get; set; }

        [JsonPropertyName("permissions")]
        public DiscordPermission Permissions { get; set; }
    }
}
