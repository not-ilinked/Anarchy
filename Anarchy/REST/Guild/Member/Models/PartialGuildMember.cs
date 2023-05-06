using System;
using System.Text.Json.Serialization;

namespace Discord
{
    public class PartialGuildMember : Controllable, IDisposable
    {
        public PartialGuildMember()
        {
            OnClientUpdated += (sender, e) => User.SetClient(Client);
        }

        [JsonPropertyName("guild_id")]
        internal ulong GuildId { get; set; }

        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(GuildId).SetClient(Client);
            }
        }

        [JsonPropertyName("user")]
        public DiscordUser User { get; internal set; }

        public new void Dispose()
        {
            User = null;
            base.Dispose();
        }
    }
}
