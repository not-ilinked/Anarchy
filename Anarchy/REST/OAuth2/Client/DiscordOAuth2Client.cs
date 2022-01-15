using Discord.Gateway;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Discord
{
    public class DiscordOAuth2Client
    {
        private readonly ulong _clientId;
        private readonly string _clientSecret;
        private DiscordOAuth2Authorization _auth;

        private readonly HttpClient _httpClient;
        private readonly DiscordSocketClient _botClient;

        public string RefreshToken => _auth.RefreshToken;
        public string[] Scopes => _auth.Scopes;

        public DiscordOAuth2Client(ulong clientId, string clientSecret, DiscordSocketClient botClient = null)
        {
            if (botClient != null && botClient.User.Type != DiscordUserType.Bot)
            {
                throw new ArgumentException("The client must be using a bot account", "botClient");
            }

            _botClient = botClient;

            _clientId = clientId;
            _clientSecret = clientSecret;

            _httpClient = new HttpClient();
        }

        private void authorize(string grantType, Dictionary<string, string> useSpecific)
        {
            Dictionary<string, string> values = new Dictionary<string, string>()
            {
                { "client_id", _clientId.ToString() },
                { "client_secret", _clientSecret },
                { "grant_type", grantType }
            };

            foreach (string key in useSpecific.Keys)
            {
                values[key] = useSpecific[key];
            }

            HttpResponseMessage resp = _httpClient.PostAsync(DiscordHttpUtil.BuildBaseUrl(9, DiscordReleaseChannel.Stable) + "/oauth2/token", new FormUrlEncodedContent(values)).Result;

            if (resp.StatusCode >= HttpStatusCode.BadRequest)
            {
                throw new OAuth2Exception(JsonConvert.DeserializeObject<OAuth2HttpError>(resp.Content.ReadAsStringAsync().Result));
            }

            _auth = JsonConvert.DeserializeObject<DiscordOAuth2Authorization>(resp.Content.ReadAsStringAsync().Result);
        }

        public void Refresh(string refreshToken)
        {
            authorize("refresh_token", new Dictionary<string, string>() { { "refresh_token", refreshToken } });
        }

        public void Authorize(string code, string redirectUri)
        {
            authorize("authorization_code", new Dictionary<string, string>() { { "code", code }, { "redirect_uri", redirectUri } });
        }

        private void EnsureAuth()
        {
            if (_auth == null)
            {
                throw new InvalidOperationException("You must authenticate before making this request");
            }
            else if (_auth.ExpiresAt < DateTime.UtcNow)
            {
                Refresh(_auth.RefreshToken);
            }
        }


        private T Request<T>(string method, string endpoint, object data = null)
        {
            EnsureAuth();

            HttpRequestMessage req = new HttpRequestMessage()
            {
                Method = new HttpMethod(method),
                RequestUri = new Uri(DiscordHttpUtil.BuildBaseUrl(9, DiscordReleaseChannel.Stable) + endpoint),
                Content = data == null ? null : new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };

            req.Headers.Add("Authorization", $"{_auth.TokenType} {_auth.AccessToken}");

            HttpResponseMessage resp = _httpClient.SendAsync(req).GetAwaiter().GetResult();
            JToken bodyObj = JToken.Parse(resp.Content.ReadAsStringAsync().Result);

            DiscordHttpUtil.ValidateResponse((int)resp.StatusCode, bodyObj);

            return bodyObj.ToObject<T>();
        }


        public DiscordUser GetUser()
        {
            if (!_auth.Scopes.Contains("identify") && !_auth.Scopes.Contains("email"))
            {
                throw new InvalidOperationException("You must have the 'identify' or 'email' scope to make this request");
            }

            DiscordUser user = Request<DiscordUser>("GET", "/users/@me");
            if (_botClient != null)
            {
                user.SetClient(_botClient);
            }

            return user;
        }

        public IReadOnlyList<ConnectedAccount> GetConnectedAccounts()
        {
            if (!_auth.Scopes.Contains("connections"))
            {
                throw new InvalidOperationException("You must have the 'connections' scope to make this request");
            }

            List<ConnectedAccount> connections = Request<List<ConnectedAccount>>("GET", "/users/@me/connections");
            if (_botClient != null)
            {
                connections.SetClientsInList(_botClient);
            }

            return connections;
        }

        public IReadOnlyList<PartialGuild> GetGuilds()
        {
            if (!_auth.Scopes.Contains("guilds"))
            {
                throw new InvalidOperationException("You must have the 'guilds' scope to make this request");
            }

            List<PartialGuild> guilds = Request<List<PartialGuild>>("GET", "/users/@me/guilds");
            if (_botClient != null)
            {
                guilds.SetClientsInList(_botClient);
            }

            return guilds;
        }

        public GuildMember JoinGuild(ulong guildId, OAuth2GuildJoinProperties properties = null)
        {
            if (!_auth.Scopes.Contains("guilds.join"))
            {
                throw new InvalidOperationException("You must have the 'guilds.join' scope to make this request");
            }

            if (_botClient == null)
            {
                throw new InvalidOperationException("You must specify a DiscordSocketClient when constructing to use this method");
            }

            EnsureAuth();

            if (properties == null)
            {
                properties = new OAuth2GuildJoinProperties();
            }

            properties.AccessToken = _auth.AccessToken;

            DiscordHttpResponse resp = _botClient.HttpClient.PutAsync($"/guilds/{guildId}/members/{GetUser().Id}", properties).GetAwaiter().GetResult();

            if (resp.Body == null)
            {
                return null;
            }
            else
            {
                return resp.Deserialize<GuildMember>().SetClient(_botClient);
            }
        }
    }
}
