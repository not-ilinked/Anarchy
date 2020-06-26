using Newtonsoft.Json;

namespace Discord
{
    public class MinimalChannel : ControllableEx
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        public MinimalChannel()
        { }

        public MinimalChannel(ulong channelId)
        {
            Id = channelId;
        }

        /// <summary>
        /// Deletes the channel
        /// </summary>
        /// <returns>The deleted <see cref="DiscordChannel"/></returns>
        public void Delete()
        {
            Client.DeleteChannel(Id);
        }
    }
}
