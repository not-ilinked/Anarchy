namespace Discord.Gateway
{
    public enum GatewayOpcode
    {
        Event,
        Heartbeat,
        Identify,
        PresenceChange,
        VoiceStateUpdate,
        Resume = 6,
        Reconnect,
        RequestGuildMembers,
        InvalidSession,
        Connected,
        HeartbeatAck,
        ReportOpenDM = 13, // seems like some data collection bs idk
        GuildSubscriptions = 14,
        GoLive = 18,
        EndGoLive,
        WatchGoLive,
        GoLiveUpdate = 22
    }
}