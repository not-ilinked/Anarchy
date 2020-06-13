using Newtonsoft.Json;
using System;

namespace Discord
{
    public class DiscordEmoji : PartialEmoji
    {
        public DiscordEmoji()
        {
            OnClientUpdated += (sender, e) => Creator.SetClient(Client);
        }


        [JsonProperty("user")]
        public DiscordUser Creator { get; private set; }


        [JsonProperty("available")]
        public bool Available { get; private set; }


        internal ulong GuildId { get; set; }

        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(GuildId).SetClient(Client);
            }
        }

        /// <summary>
        /// Updates the emoji's info
        /// </summary>
        public void Update()
        {
            Name = Client.GetGuildEmoji(GuildId, (ulong)Id).Name;
        }


        /// <summary>
        /// Modifies the emoji
        /// </summary>
        /// <param name="name">New name</param>
        public void Modify(string name)
        {
            Name = Client.ModifyEmoji(GuildId, (ulong)Id, name).Name;
        }


        /// <summary>
        /// Deletes the emoji
        /// </summary>
        public void Delete()
        {
            Client.DeleteEmoji(GuildId, (ulong)Id);
        }
    }
}
