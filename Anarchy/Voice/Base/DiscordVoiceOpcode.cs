namespace Discord.Voice
{
    public enum DiscordVoiceOpcode
    { 
        Identify,
        SelectProtocol,
        Ready,
        Heartbeat,
        SessionDescription,
        Speaking,
        HeartbeatAck,
        Resume,
        Hello,
        Resumed,
        SSRCUpdate = 12,
        ClientDisconnect
    }
}
