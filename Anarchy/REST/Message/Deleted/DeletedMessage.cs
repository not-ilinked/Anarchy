using Newtonsoft.Json;

namespace Discord
{
    public class DeletedMessage
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("channel_id")]
        public ulong ChannelId { get; private set; }


        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }


        public static implicit operator ulong(DeletedMessage instance)
        {
            return instance.Id;
        }
    }
}
