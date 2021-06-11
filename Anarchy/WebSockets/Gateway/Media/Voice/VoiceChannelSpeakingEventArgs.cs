namespace Discord.Media
{
    public class VoiceChannelSpeakingEventArgs
    {
        public DiscordVoiceClient Client { get; }
        public IncomingVoiceStream Stream { get; }

        public VoiceChannelSpeakingEventArgs(DiscordVoiceClient client, IncomingVoiceStream stream)
        {
            Client = client;
            Stream = stream;
        }
    }
}
