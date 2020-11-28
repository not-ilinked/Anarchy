using Anarchy;

namespace Discord.Gateway
{
    internal class VoiceStateDictionary : ConcurrentDictionary<ulong, DiscordVoiceStateContainer>
    {
        public new DiscordVoiceStateContainer this[ulong userId]
        {
            get
            {
                if (TryGetValue(userId, out DiscordVoiceStateContainer container))
                    return container;
                else
                    return this[userId] = new DiscordVoiceStateContainer(userId);
            }
            set { base[userId] = value; }
        }
    }
}
