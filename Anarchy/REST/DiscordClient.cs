using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace Discord
{
    /// <summary>
    /// Discord client that only supports HTTP
    /// </summary>
    public class DiscordClient
    {
        public DiscordHttpClient HttpClient { get; private set; }
        public DiscordClientUser User { get; internal set; }

        private string _token;
        public string Token
        {
            get
            {
                return _token;
            }
            set
            {
                string previousToken = Token;

                _token = value;

                try
                {
                    this.GetClientUser();
                }
                catch (DiscordHttpException ex)
                {
                    _token = previousToken;

                    if (ex.Code == DiscordError.GeneralError && ex.ErrorMessage == "401: Unauthorized")
                        throw new InvalidTokenException(value);
                    else
                        throw;
                }
            }
        }


        public DiscordConfig Config { get; private set; }


        public DiscordClient(DiscordConfig config = null)
        {
            if (config == null)
                config = new DiscordConfig();

            config.SuperProperties.Base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(config.SuperProperties)));

            Config = config;

            HttpClient = new DiscordHttpClient(this);
        }

        public DiscordClient(string token, DiscordConfig config = null) : this(config)
        {
            Token = token;
        }


        public override string ToString()
        {
            return User.ToString();
        }
    }
}