using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord
{
    public class InviteGuild : BaseGuild
    {
        [JsonProperty("description")]
        public string Description { get; protected set; }

        [JsonProperty("splash")]
        private string _splashHash;

        public DiscordCDNImage Splash
        {
            get
            {
                if (_splashHash == null)
                    return null;
                else
                    return new DiscordCDNImage(CDNEndpoints.Splash, Id, _splashHash);
            }
        }

        [JsonProperty("features")]
        public IReadOnlyList<string> Features { get; private set; }

        [JsonProperty("vanity_url_code")]
        public string VanityInvite { get; private set; }

        [JsonProperty("verification_level")]
        public GuildVerificationLevel VerificationLevel { get; private set; }

        protected void Update(InviteGuild guild)
        {
            base.Update(guild);

            Description = guild.Description;
            _splashHash = guild._splashHash;
            Features = guild.Features;
            VanityInvite = guild.VanityInvite;
            VerificationLevel = guild.VerificationLevel;
        }
    }
}
