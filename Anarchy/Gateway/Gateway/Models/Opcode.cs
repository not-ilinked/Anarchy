namespace Discord.Gateway
{
    internal enum GatewayOpcode
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
        RequestGuildMembersUser = 14,
        /*
        GoLive = 18,
        EndGoLive = 19,
        GoLiveUpdate = 22*/
    }
}