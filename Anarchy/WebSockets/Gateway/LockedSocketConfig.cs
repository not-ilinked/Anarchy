namespace Discord.Gateway
{
    public class LockedSocketConfig : LockedDiscordConfig
    {
        public bool Cache { get; private set; }
        public DiscordGatewayIntent? Intents { get; private set; }
        public int VoiceChannelConnectTimeout { get; private set; }
        public DiscordShard Shard { get; private set; }

        public LockedSocketConfig(DiscordSocketConfig config) : base(config)
        {
            Cache = config.Cache;
            Intents = config.Intents;
            VoiceChannelConnectTimeout = config.VoiceChannelConnectTimeout;
            Shard = config.Shard;
        }
    }
}
