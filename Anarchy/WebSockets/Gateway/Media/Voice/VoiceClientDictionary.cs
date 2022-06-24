using System.Collections.Generic;
using Discord.Media;

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
