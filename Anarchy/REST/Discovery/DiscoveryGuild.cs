using Discord.Gateway;
using Newtonsoft.Json;
using System;

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
        public string PreferredLocale { get; private set; }


        public DiscordGuild Lurk()
        {
            if (SocketClient)
                return ((DiscordSocketClient)Client).LurkGuild(Id);
            else
                throw new NotSupportedException("This method is only available for socket clients.");
        }
    }
}
