namespace Discord.Gateway
{
    public static class GatewayIntentBundles
    {
        public static DiscordGatewayIntent DirectMessages = DiscordGatewayIntent.DirectMessages | DiscordGatewayIntent.DirectMessageReactions | DiscordGatewayIntent.DirectMessageTyping;

        public static DiscordGatewayIntent GuildAdministration = DiscordGatewayIntent.Guilds | DiscordGatewayIntent.GuildWebhooks | DiscordGatewayIntent.GuildInvites | DiscordGatewayIntent.GuildIntegrations | DiscordGatewayIntent.GuildBans;

        public static DiscordGatewayIntent GuildMessages = DiscordGatewayIntent.GuildMessageTyping | DiscordGatewayIntent.GuildMessages | DiscordGatewayIntent.GuildMessageReactions | DiscordGatewayIntent.MessageContent;

        public static DiscordGatewayIntent Guilds = GuildAdministration | GuildMessages | DiscordGatewayIntent.GuildVoiceStates;
    }
}