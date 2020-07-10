using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class ActiveSessionPlatforms
    {
        [JsonProperty("desktop")]
        private readonly string _desktop;

        public UserStatus Desktop
        {
            get
            {
                return UserStatusConverter.FromString(_desktop);
            }
        }


        [JsonProperty("web")]
        private readonly string _web;

        public UserStatus Website
        {
            get
            {
                return UserStatusConverter.FromString(_web);
            }
        }


        [JsonProperty("mobile")]
        private readonly string _mobile;

        public UserStatus Mobile
        {
            get
            {
                return UserStatusConverter.FromString(_mobile);
            }
        }
    }
}
