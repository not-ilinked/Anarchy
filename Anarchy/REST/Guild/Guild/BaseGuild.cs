using Newtonsoft.Json;
using System;
using System.Drawing;

namespace Discord
{
    public abstract class BaseGuild : MinimalGuild
    {
        [JsonProperty("name")]
        public string Name { get; protected set; }


        [JsonProperty("icon")]
        protected string _iconId;


        public DiscordGuildIcon Icon
        {
            get
            {
                return new DiscordGuildIcon(Id, _iconId);
            }
        }


        /// <summary>
        /// Gets the guild's icon
        /// </summary>
        /// <returns>The guild's icon (returns null if IconId is null)</returns>
        [Obsolete("GetIcon is obsolete. Use Icon.Download() instead", true)]
        public Image GetIcon()
        {
            return null;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
