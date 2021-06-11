using Discord.Media;
using System.Collections.Generic;

namespace Discord.Gateway
{
    public class VoiceClientDictionary : Dictionary<ulong, DiscordVoiceClient>
    {
        public VoiceClientDictionary(DiscordSocketClient client) : base()
        {
            Private = new DiscordVoiceClient(client, 0);
        }

        public DiscordVoiceClient Private { get; }
    }
}
