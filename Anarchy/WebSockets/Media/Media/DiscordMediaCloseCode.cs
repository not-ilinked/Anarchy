namespace Discord.Media
{
    public enum DiscordMediaCloseCode
    {
        UnknownOpcode = 4001,
        FailedToDecodePayload,
        NotAuthenticated,
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
