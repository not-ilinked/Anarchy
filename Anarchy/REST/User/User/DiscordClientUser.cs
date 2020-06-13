using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Account user
    /// </summary>
    public class DiscordClientUser : DiscordUser
    {
        [JsonProperty("token")]
        internal string Token { get; private set; }


        [JsonProperty("email")]
        public string Email { get; private set; }


        [JsonProperty("verified")]
        public bool EmailVerified { get; private set; }


        [JsonProperty("mfa_enabled")]
        public bool TwoFactorAuth { get; private set; }


        [JsonProperty("explicit_content_filter")]
        public ExplicitContentFilter ExplicitContentFilter { get; private set; }


        [JsonProperty("locale")]
        private string _locale;

        public DiscordLanguage Language
        {
            get { return LanguageUtils.StringToLanguage(_locale); }
        }

        [JsonProperty("premium_type")]
        private DiscordNitroType? _nitro;

        public DiscordNitroType Nitro
        {
            get { return _nitro ?? DiscordNitroType.None; }
        }


        internal void Update(DiscordClientUser user)
        {
            base.Update(user);
            Email = user.Email;
            EmailVerified = user.EmailVerified;
            TwoFactorAuth = user.TwoFactorAuth;
            ExplicitContentFilter = user.ExplicitContentFilter;
            _locale = user._locale;
            _nitro = user._nitro;
        }


        /// <summary>
        /// Updates the user's info
        /// </summary>
        public new void Update()
        {
            Update(Client.GetClientUser());
        }


        /// <summary>
        /// Changes the user's profile
        /// </summary>
        /// <param name="settings">Options for changing the profile</param>
        public void ChangeProfile(UserProfileUpdate settings)
        {
            if (settings.Email == null)
                settings.Email = Email;
            if (!settings.DiscriminatorProperty.Set)
                settings.Discriminator = Discriminator;
            if (settings.Username == null)
                settings.Username = Username;

            DiscordClientUser user = Client.HttpClient.Patch("/users/@me", settings).Deserialize<DiscordClientUser>();

            Update(user);

            if (user.Token != null)
                Client.Token = user.Token;
        }


        /// <summary>
        /// Gets the users settings
        /// </summary>
        public DiscordUserSettings GetSettings()
        {
            return Client.HttpClient.Get("/users/@me/settings")
                                .Deserialize<DiscordUserSettings>();
        }


        /// <summary>
        /// Changes the user's settings
        /// </summary>
        public DiscordUserSettings ChangeSettings(UserSettingsProperties settings)
        {
            return Client.HttpClient.Patch("/users/@me/settings", settings)
                                .Deserialize<DiscordUserSettings>();
        }


        /// <summary>
        /// Deletes the account
        /// </summary>
        /// <param name="password">The account's password</param>
        public void Delete(string password)
        {
            Client.HttpClient.Post("/users/@me/delete", $"{{\"password\":\"{password}\"}}");
        }


        /// <summary>
        /// Disables the account
        /// </summary>
        /// <param name="password">The account's password</param>
        public void Disable(string password)
        {
            Client.HttpClient.Post("/users/@me/disable", $"{{\"password\":\"{password}\"}}");
        }


        /// <summary>
        /// Sets the account's hypesquad
        /// </summary>
        public void SetHypesquad(Hypesquad house)
        {
            if (house == Hypesquad.None)
                Client.HttpClient.Delete("/hypesquad/online");
            else
                Client.HttpClient.Post("/hypesquad/online", $"{{\"house_id\":{(int)house}}}");
        }
    }
}
