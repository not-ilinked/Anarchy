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
        /// Modifies the channel
        /// </summary>
        /// <param name="properties">Options for modifying the channel</param>
        public DiscordChannel Modify(string name)
        {
            return Client.ModifyChannel(Id, name);
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
