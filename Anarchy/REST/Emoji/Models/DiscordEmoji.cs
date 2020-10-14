using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

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

        private void Update(DiscordEmoji emoji)
        {
            Name = emoji.Name;
        }

        public async Task UpdateAsync()
        {
            Update(await Client.GetGuildEmojiAsync(GuildId, (ulong)Id));
        }

        /// <summary>
        /// Updates the emoji's info
        /// </summary>
        public void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }


        public async Task ModifyAsync(string name)
        {
            Update(await Client.ModifyEmojiAsync(GuildId, (ulong)Id, name));
        }

        /// <summary>
        /// Modifies the emoji
        /// </summary>
        /// <param name="name">New name</param>
        public void Modify(string name)
        {
            ModifyAsync(name).GetAwaiter().GetResult();
        }


        public async Task DeleteAsync()
        {
            await Client.DeleteEmojiAsync(GuildId, (ulong)Id);
        }

        /// <summary>
        /// Deletes the emoji
        /// </summary>
        public void Delete()
        {
            DeleteAsync().GetAwaiter().GetResult();
        }
    }
}
