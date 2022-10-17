using System;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class InviteCreatedEventArgs : Controllable
    {
        [JsonProperty("code")]
        public string Code { get; private set; }

        [JsonProperty("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel
        {
            get { return new MinimalTextChannel(_channelId).SetClient(Client); }
        }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; private set; }

        [JsonProperty("guild_id")]
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

        [JsonProperty("inviter")]
        public DiscordUser Inviter { get; private set; }

        [JsonProperty("max_age")]
        public uint MaxAge { get; private set; }

        [JsonProperty("max_uses")]
        public uint MaxUses { get; private set; }

        [JsonProperty("temporary")]
        public bool Temporary { get; private set; }
    }
}
