using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class InviteGuild : BaseGuild
    {
        [JsonPropertyName("description")]
        public string Description { get; protected set; }

        [JsonPropertyName("splash")]
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

        [JsonPropertyName("features")]
        public IReadOnlyList<string> Features { get; private set; }

        [JsonPropertyName("vanity_url_code")]
        public string VanityInvite { get; private set; }

        [JsonPropertyName("verification_level")]
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
