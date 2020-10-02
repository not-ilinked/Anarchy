namespace Discord.Media
{
    public enum DiscordMediaCloseCode : ushort
    {
        // Anarchy
        ClosedByClient = 3000,
        UnknownCodec,
        InvalidEncryptionMode,

        // Discord
        UnknownOpcode = 4001,
        NotAuthenticated = 4003,
        AuthenticationFailed,
        AlreadyAuthenticated,
        InvalidSession,
        SessionTimeout = 4009,
        ServerNotFound = 4011,
        UnknownProtocol,
        Disconnected = 4014,
        ServerCrashed,
        UnknownEncryptionMode
    }
}