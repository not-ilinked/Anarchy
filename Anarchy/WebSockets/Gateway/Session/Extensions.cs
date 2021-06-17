namespace Discord.Gateway
{
    public static class GatewayAuthExtensions
    {
        /// <summary>
        /// Logs intot he gateway
        /// </summary>
        internal static void LoginToGateway(this DiscordSocketClient client)
        {
            client.Send(GatewayOpcode.Identify, new GatewayIdentification()
            {
                Token = client.Token,
                Properties = client.Config.SuperProperties,
                Intents = client.User.Type == DiscordUserType.Bot ? client.Config.Intents : null,
                Shard = client.Config.Shard
            });
        }


        internal static void Resume(this DiscordSocketClient client)
        {
            client.Send(GatewayOpcode.Resume, new GatewayResume(client));
        }
    }
}
