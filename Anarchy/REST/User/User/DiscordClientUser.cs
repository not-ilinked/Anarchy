using Newtonsoft.Json;
using System.Threading.Tasks;

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
            get { return LanguageConverter.FromString(_locale); }
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


        public async Task ChangeProfileAsync(UserProfileUpdate settings)
        {
            if (settings.Email == null)
                settings.Email = Email;
            if (!settings.DiscriminatorProperty.Set)
                settings.Discriminator = Discriminator;
            if (settings.Username == null)
                settings.Username = Username;

            DiscordClientUser user = (await Client.HttpClient.PatchAsync("/users/@me", settings)).Deserialize<DiscordClientUser>();

            Update(user);

            if (user.Token != null)
                Client.Token = user.Token;
        }

        /// <summary>
        /// Changes the user's profile
        /// </summary>
        /// <param name="settings">Options for changing the profile</param>
        public void ChangeProfile(UserProfileUpdate settings)
        {
            ChangeProfileAsync(settings).GetAwaiter().GetResult();
        }


        public async Task<DiscordUserSettings> GetSettingsAsync()
        {
            return (await Client.HttpClient.GetAsync("/users/@me/settings"))
                                .Deserialize<DiscordUserSettings>().SetClient(Client);
        }

        /// <summary>
        /// Gets the users settings
        /// </summary>
        public DiscordUserSettings GetSettings()
        {
            return GetSettingsAsync().GetAwaiter().GetResult();
        }


        public async Task<DiscordUserSettings> ChangeSettingsAsync(UserSettingsProperties settings)
        {
            return (await Client.HttpClient.PatchAsync("/users/@me/settings", settings))
                                .Deserialize<DiscordUserSettings>().SetClient(Client);
        }

        /// <summary>
        /// Changes the user's settings
        /// </summary>
        public DiscordUserSettings ChangeSettings(UserSettingsProperties settings)
        {
            return ChangeSettingsAsync(settings).GetAwaiter().GetResult();
        }


        public async Task DeleteAsync(string password)
        {
            await Client.HttpClient.PostAsync("/users/@me/delete", $"{{\"password\":\"{password}\"}}");
        }

        /// <summary>
        /// Deletes the account
        /// </summary>
        /// <param name="password">The account's password</param>
        public void Delete(string password)
        {
            DeleteAsync(password).GetAwaiter().GetResult();
        }


        public async Task DisableAsync(string password)
        {
            await Client.HttpClient.PostAsync("/users/@me/disable", $"{{\"password\":\"{password}\"}}");
        }

        /// <summary>
        /// Disables the account
        /// </summary>
        /// <param name="password">The account's password</param>
        public void Disable(string password)
        {
            DisableAsync(password).GetAwaiter().GetResult();
        }


        public async Task SetHypesquadAsync(Hypesquad house)
        {
            if (house == Hypesquad.None)
                await Client.HttpClient.DeleteAsync("/hypesquad/online");
            else
                await Client.HttpClient.PostAsync("/hypesquad/online", $"{{\"house_id\":{(int)house}}}");
        }

        /// <summary>
        /// Sets the account's hypesquad
        /// </summary>
        public void SetHypesquad(Hypesquad house)
        {
            SetHypesquadAsync(house).GetAwaiter().GetResult();
        }
    }
}
