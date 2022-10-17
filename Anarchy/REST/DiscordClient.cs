namespace Discord
{
    /// <summary>
    /// Discord client that only supports HTTP
    /// </summary>
    public class DiscordClient
    {
        public DiscordClientUser User { get; internal set; }
        public LockedDiscordConfig Config { get; protected set; }
        public DiscordHttpClient HttpClient { get; private set; }

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

                    if (ex.Code == DiscordError.MessageOnlyError && ex.ErrorMessage == "401: Unauthorized")
                        throw new InvalidTokenException(value);
                    else
                        throw;
                }
            }
        }

        protected DiscordClient()
        {
            HttpClient = new DiscordHttpClient(this);
        }

        public DiscordClient(ApiConfig config = null) : this()
        {
            if (config == null)
                config = new ApiConfig();

            Config = new LockedDiscordConfig(config);
        }

        public DiscordClient(string token, ApiConfig config = null) : this(config)
        {
            Token = token;
        }

        public override string ToString()
        {
            return User.ToString();
        }
    }
}