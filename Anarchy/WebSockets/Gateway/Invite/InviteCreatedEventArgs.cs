using System;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class InviteCreatedEventArgs : Controllable
    {
        [JsonPropertyName("code")]
        public string Code { get; private set; }

        [JsonPropertyName("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel
        {
            get { return new MinimalTextChannel(_channelId).SetClient(Client); }
        }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; private set; }

        [JsonPropertyName("guild_id")]
        private readonly ulong? _guildId;

        public MinimalGuild Guild
        {
            get
            {
                if (_guildId.HasValue)
                    return new MinimalGuild(_guildId.Value).SetClient(Client);
                else
                    return null;
            }
        }

        [JsonPropertyName("inviter")]
        public DiscordUser Inviter { get; private set; }

        [JsonPropertyName("max_age")]
        public uint MaxAge { get; private set; }

        [JsonPropertyName("max_uses")]
        public uint MaxUses { get; private set; }

        [JsonPropertyName("temporary")]
        public bool Temporary { get; private set; }
    }
}
