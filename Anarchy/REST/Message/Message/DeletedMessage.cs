using Newtonsoft.Json;

namespace Discord
{
    public class DeletedMessage : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel => new MinimalTextChannel(_channelId).SetClient(Client);


        [JsonProperty("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild => new MinimalGuild(_guildId).SetClient(Client);


        public static implicit operator ulong(DeletedMessage instance)
        {
            return instance.Id;
        }
    }
}
