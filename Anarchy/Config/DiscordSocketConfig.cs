namespace Discord.Gateway
{
    public class DiscordSocketConfig : DiscordConfig
    {
        public bool Cache { get; set; } = true;
        public DiscordGatewayIntent? Intents { get; set; }
    }
}
