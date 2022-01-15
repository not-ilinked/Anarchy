using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class OAuth2GuildJoinProperties
    {
        [JsonProperty("access_token")]
        internal string AccessToken { get; set; }

        private readonly DiscordParameter<string> _nickParam = new DiscordParameter<string>();
        [JsonProperty("nick")]
        public string Nickname
        {
            get => _nickParam;
            set => _nickParam.Value = value;
        }

        public bool ShouldSerializeNickname()
        {
            return _nickParam.Set;
        }

        private readonly DiscordParameter<List<ulong>> _roleParam = new DiscordParameter<List<ulong>>();
        [JsonProperty("roles")]
        public List<ulong> Roles
        {
            get => _roleParam;
            set => _roleParam.Value = value;
        }

        public bool ShouldSerializeRoles()
        {
            return _roleParam.Set;
        }

        private readonly DiscordParameter<bool> _muteParam = new DiscordParameter<bool>();
        [JsonProperty("mute")]
        public bool Mute
        {
            get => _muteParam;
            set => _muteParam.Value = value;
        }

        public bool ShouldSerializeMute()
        {
            return _muteParam.Set;
        }

        private readonly DiscordParameter<bool> _deafParam = new DiscordParameter<bool>();
        [JsonProperty("deaf")]
        public bool Deaf
        {
            get => _deafParam;
            set => _deafParam.Value = value;
        }

        public bool ShouldSerializeDeaf()
        {
            return _deafParam.Set;
        }
    }
}
