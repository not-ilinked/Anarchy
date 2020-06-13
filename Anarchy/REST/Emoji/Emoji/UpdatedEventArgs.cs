using System.Collections.Generic;

namespace Discord.Gateway
{
    public class EmojisUpdatedEventArgs
    {
        public ulong GuildId { get; private set; }
        public IReadOnlyList<DiscordEmoji> Emojis { get; private set; }

        internal EmojisUpdatedEventArgs(EmojiContainer emojis)
        {
            GuildId = emojis.GuildId;
            Emojis = emojis.Emojis;
        }
    }
}
