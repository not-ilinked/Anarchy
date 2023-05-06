﻿using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discord
{
    public class MinimalChannel : Controllable
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }
        public MinimalChannel()
        { }

        public MinimalChannel(ulong channelId)
        {
            Id = channelId;
        }

        public async Task DeleteAsync()
        {
            await Client.DeleteChannelAsync(Id);
        }

        /// <summary>
        /// Deletes the channel
        /// </summary>
        /// <returns>The deleted <see cref="DiscordChannel"/></returns>
        public void Delete()
        {
            DeleteAsync().GetAwaiter().GetResult();
        }
    }
}