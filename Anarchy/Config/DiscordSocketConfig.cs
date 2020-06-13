namespace Discord.Gateway
{
    public class DiscordSocketConfig : DiscordConfig
    {
        public bool ConnectToVoiceChannels { get; set; } = true;
        public bool GuildSubscriptions { get; set; } = true;
        public bool Cache { get; set; } = true;
        internal DiscordGatewayIntents Intents { get; set; } = new DiscordGatewayIntents(GatewayIntentCalculator.All());
    }
}
