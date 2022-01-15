using Newtonsoft.Json;
using System;

namespace Discord
{
    internal class DiscordOAuth2Authorization
    {
        [JsonProperty("token_type")]
        public string TokenType { get; private set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; private set; }

        [JsonProperty("expires_in")]
        private int _expiresIn
        {
            set => ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(value);
        }

        public DateTimeOffset ExpiresAt { get; private set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; private set; }

        [JsonProperty("scope")]
        private readonly string _scope;
        public string[] Scopes => _scope.Split(' ');
    }
}
