namespace Discord.Gateway
{
    public enum GatewayCloseError
    {
        UnknownError = 4000,
        UnknownOpcode,
        DecodeError,
        NotAuthenticated,
        AuthenticationFailed,
        AlreadyAuthenticated,
        InvalidSequence = 4007,
        RateLimited,
        SessionTimedOut,
        InvalidShard,
        ShardingRequired,
        InvalidAPIVersion,
        InvalidIntents,
        DisallowedIntents
    }
}
