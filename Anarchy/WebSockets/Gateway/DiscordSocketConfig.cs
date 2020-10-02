namespace Discord.Gateway
{
    public class DiscordSocketConfig : DiscordConfig
    {
        public bool Cache { get; set; } = true;
        public DiscordGatewayIntent? Intents { get; set; }
        public int VoiceChannelConnectTimeout { get; set; } = 10 * 1000;
        public DiscordShard Shard { get; set; }
    }
}
