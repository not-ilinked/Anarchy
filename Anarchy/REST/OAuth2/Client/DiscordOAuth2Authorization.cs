using System;
using System.Text.Json.Serialization;

namespace Discord
{
    internal class DiscordOAuth2Authorization
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; private set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; private set; }

        [JsonPropertyName("expires_in")]
        private int _expiresIn
        {
            set => ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(value);
        }

        public DateTimeOffset ExpiresAt { get; private set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; private set; }

        [JsonPropertyName("scope")]
        private readonly string _scope;
        public string[] Scopes => _scope.Split(' ');
    }
}
