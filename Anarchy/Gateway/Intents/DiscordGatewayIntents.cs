namespace Discord.Gateway
{
    public class DiscordGatewayIntents
    {
        public DiscordGatewayIntent Intents { get; private set; }

        public DiscordGatewayIntents(DiscordGatewayIntent intents)
        {
            Intents = intents;
        }


        public void Add(DiscordGatewayIntent intent)
        {
            Intents = GatewayIntentCalculator.Add(Intents, intent);
        }


        public void Remove(DiscordGatewayIntent intent)
        {
            Intents = GatewayIntentCalculator.Remove(Intents, intent);
        }


        public bool Has(DiscordGatewayIntent intent)
        {
            return GatewayIntentCalculator.Has(Intents, intent);
        }
    }
}
