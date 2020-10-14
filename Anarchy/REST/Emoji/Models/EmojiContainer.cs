using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord.Gateway
{
    internal class EmojiContainer : Controllable
    {
        public EmojiContainer()
        {
            OnClientUpdated += (sender, e) => 
            {
                Emojis.SetClientsInList(Client);

                foreach (var emoji in Emojis)
                    emoji.GuildId = GuildId;
            };
        }


        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }


        [JsonProperty("emojis")]
        public IReadOnlyList<DiscordEmoji> Emojis { get; private set; }
    }
}
