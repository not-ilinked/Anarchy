

using System.Text.Json.Serialization;

namespace Discord
{
    public class DeletedMessage : Controllable
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel => new MinimalTextChannel(_channelId).SetClient(Client);

        [JsonPropertyName("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(_guildId).SetClient(Client);
            }
        }

        public static implicit operator ulong(DeletedMessage instance)
        {
            return instance.Id;
        }
    }
}
