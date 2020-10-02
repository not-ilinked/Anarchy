using System;

namespace Discord.Media
{
    [Flags]
    public enum DiscordSpeakingFlags
    {
        NotSpeaking = 0,
        Microphone = 1 << 0,
        Soundshare = 1 << 1,
        Priority = 1 << 2
    }
}
