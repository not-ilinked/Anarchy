using Newtonsoft.Json;

namespace Discord
{
    internal class LoginRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }


        [JsonProperty("password")]
        public string Password { get; set; }


        [JsonProperty("captcha_key")]
        public string CaptchaKey { get; set; }
    }
}
