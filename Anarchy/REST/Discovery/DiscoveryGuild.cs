using Discord.Gateway;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    public class DiscoveryGuild : BaseGuild
    {
        [JsonProperty("description")]
        public string Description { get; protected set; }


        [JsonProperty("approximate_presence_count")]
        public int OnlineMembers { get; private set; }


        [JsonProperty("approximate_member_count")]
        public int Members { get; private set; }


        [JsonProperty("premium_subscription_count")]
        public int PremiumSubscriptions { get; private set; }


        [JsonProperty("preferred_locale")]
        public DiscordLanguage PreferredLanguage { get; private set; }


        [JsonProperty("discovery_splash")]
        private readonly string _discoverySplashHash;

        public DiscordCDNImage DiscoverySplash
        {
            get
            {
                if (_discoverySplashHash == null)
                    return null;
                else
                    return new DiscordCDNImage(CDNEndpoints.DiscoverySplash, Id, _discoverySplashHash);
            }
        }


        [JsonProperty("emojis")]
        public IReadOnlyList<DiscordEmoji> Emojis { get; private set; }


        [JsonProperty("keywords")]
        public IReadOnlyList<string> Keywords { get; private set; }


        [JsonProperty("vanity_url_code")]
        public string VanityInvite { get; private set; }


        public async Task<DiscordGuild> LurkAsync()
        {
            if (Client.GetType() == typeof(DiscordSocketClient))
                return await ((DiscordSocketClient)Client).LurkGuildAsync(Id);
            else
                throw new NotSupportedException("This method is only available for socket clients.");
        }

        public DiscordGuild Lurk()
        {
            return LurkAsync().GetAwaiter().GetResult();
        }
    }
}
