using System;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class InviteCreatedEventArgs : EventArgs
    {
        [JsonProperty("code")]
        public string Code { get; private set; }


        [JsonProperty("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel
        {
            get { return new MinimalTextChannel(_channelId); }
        }


        [JsonProperty("created_at")]
        private readonly string _createdAt;

        public DateTime CreatedAt
        {
            get { return DiscordTimestamp.FromString(_createdAt); }
        }


        [JsonProperty("guild_id")]
        private readonly ulong? _guildId;

        public MinimalGuild Guild
        {
            get
            {
                if (_guildId.HasValue)
                    return new MinimalGuild(_guildId.Value);
                else
                    return null;
            }
        }


        [JsonProperty("inviter")]
        public DiscordUser Inviter { get; private set; }


        [JsonProperty("max_age")]
        public int MaxAge { get; private set; }


        [JsonProperty("max_uses")]
        public int MaxUses { get; private set; }


        [JsonProperty("temporary")]
        public bool Temporary { get; private set; }
    }
}
