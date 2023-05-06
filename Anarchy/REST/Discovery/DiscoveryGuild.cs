using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Discord.Gateway;

namespace Discord
{
    public class DiscoveryGuild : BaseGuild
    {
        [JsonPropertyName("description")]
        public string Description { get; protected set; }

        [JsonPropertyName("approximate_presence_count")]
        public int OnlineMembers { get; private set; }

        [JsonPropertyName("approximate_member_count")]
        public int Members { get; private set; }

        [JsonPropertyName("premium_subscription_count")]
        public int PremiumSubscriptions { get; private set; }

        [JsonPropertyName("preferred_locale")]
        public DiscordLanguage PreferredLanguage { get; private set; }

        [JsonPropertyName("discovery_splash")]
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

        [JsonPropertyName("emojis")]
        public IReadOnlyList<DiscordEmoji> Emojis { get; private set; }

        [JsonPropertyName("keywords")]
        public IReadOnlyList<string> Keywords { get; private set; }

        [JsonPropertyName("vanity_url_code")]
        public string VanityInvite { get; private set; }

        public async Task<DiscordGuild> LurkAsync()
        {
            if (Client.GetType() == typeof(DiscordSocketClient))
                return await ((DiscordSocketClient) Client).LurkGuildAsync(Id);
            else
                throw new NotSupportedException("This method is only available for socket clients.");
        }

        public DiscordGuild Lurk()
        {
            return LurkAsync().GetAwaiter().GetResult();
        }
    }
}
