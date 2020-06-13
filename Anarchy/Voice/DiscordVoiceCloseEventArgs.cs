namespace Discord.Voice
{
    public class DiscordVoiceCloseEventArgs
    {
        public DiscordVoiceCloseError Code { get; private set; }
        public string Reason { get; private set; }


        internal DiscordVoiceCloseEventArgs(DiscordVoiceCloseError code, string reason)
        {
            Code = code;
            Reason = reason;
        }
    }
}
