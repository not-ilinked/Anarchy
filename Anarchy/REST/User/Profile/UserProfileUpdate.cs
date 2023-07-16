using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for changing the account's profile
    /// </summary>
    public class UserProfileUpdate
    {
        private readonly DiscordParameter<string> _bioParam = new DiscordParameter<string>();
        [JsonProperty("bio")]
        public string Biography
        {
            get { return _bioParam; }
            set { _bioParam.Value = value; }
        }

        public bool ShouldSerializeBiography() => _bioParam.Set;

        private readonly DiscordParameter<string> _proParam = new DiscordParameter<string>();
        [JsonProperty("pronouns")]
        public string Pronouns
        {
            get { return _proParam; }
            set { _proParam.Value = value; }
        }

        public bool ShouldSerializePronouns() => _proParam.Set;
    }
}