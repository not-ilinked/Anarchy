using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordUserProfileUser
    {
        [JsonProperty("pronouns")]
        public string Pronouns { get; private set; }

        [JsonProperty("theme_colors")]
        public List<int> ThemeColors { get; private set; }

        [JsonProperty("popout_animation_particle_type")] // i don't know what the type/value of this is supposed to be so i'll just set it as string for now
        public string PopoutAnimationParticleType { get; private set; }

        [JsonProperty("emoji")]
        public string Emoji { get; private set; }
    }
}