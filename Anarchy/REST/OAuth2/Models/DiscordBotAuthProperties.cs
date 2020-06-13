using Newtonsoft.Json;

namespace Discord
{
    internal class DiscordBotAuthProperties
    {
        [JsonProperty("authorize")]
#pragma warning disable IDE0051, CS0414
        private readonly bool _auth = true;
#pragma warning restore IDE0051, CS0414


        [JsonProperty("bot_guild_id")]
        public ulong GuildId { get; set; }


        [JsonProperty("captcha_key")]
        public string CaptchaKey { get; set; }


        [JsonProperty("permissions")]
        public uint Permissions { get; set; }
    }
}
