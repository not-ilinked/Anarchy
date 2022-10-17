namespace Discord.Gateway
{
    public class VoiceStateEventArgs
    {
        public DiscordVoiceState State { get; private set; }

        internal VoiceStateEventArgs(DiscordVoiceState state)
        {
            State = state;
        }
    }
}
