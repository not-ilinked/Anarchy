

using System.Text.Json.Serialization;

namespace Discord
{
    /// <summary>
    /// Properties for creating an <see cref="GuildInvite"/>
    /// </summary>
    public class InviteProperties
    {
        [JsonPropertyName("max_age")]
        public uint MaxAge { get; set; }

        [JsonPropertyName("max_uses")]
        public uint MaxUses { get; set; }

        [JsonPropertyName("temporary")]
        public bool Temporary { get; set; }
    }
}