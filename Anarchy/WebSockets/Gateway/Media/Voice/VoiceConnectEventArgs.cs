using Discord.Media;
using System;

namespace Discord.Gateway
{
    public class VoiceConnectEventArgs : EventArgs
    {
        public DiscordVoiceClient Client { get; }

        public VoiceConnectEventArgs(DiscordVoiceClient client)
        {
            Client = client;
        }
    }
}
