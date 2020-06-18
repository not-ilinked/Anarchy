using Newtonsoft.Json;

namespace Discord
{
    public class DeletedMessage
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel
        {
            get
            {
                return new MinimalTextChannel(_channelId);
            }
        }


        [JsonProperty("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(_guildId);
            }
        }


        public static implicit operator ulong(DeletedMessage instance)
        {
            return instance.Id;
        }
    }
}
