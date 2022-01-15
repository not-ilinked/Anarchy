﻿using Leaf.xNet;

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
            get => _token;
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

                    if (ex.Code == DiscordError.MessageOnlyError && ex.ErrorMessage == "401: Unauthorized")
                    {
                        throw new InvalidTokenException(value);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }


        public LockedDiscordConfig Config { get; protected set; }
        public ProxyClient Proxy { get; private set; }


        protected DiscordClient()
        {
            HttpClient = new DiscordHttpClient(this);
        }


        public DiscordClient(DiscordConfig config = null) : this()
        {
            if (config == null)
            {
                config = new DiscordConfig();
            }

            Config = new LockedDiscordConfig(config);
            FinishConfig();
        }

        public DiscordClient(string token, DiscordConfig config = null) : this(config)
        {
            Token = token;
        }

        protected void FinishConfig()
        {
            if (Config.Proxy != null)
            {
                Proxy = Config.Proxy.CreateProxyClient();
            }
        }


        public override string ToString()
        {
            return User.ToString();
        }
    }
}