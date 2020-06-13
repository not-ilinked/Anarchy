using System.Collections.Generic;

namespace Discord.Gateway
{
    public static class GatewayPrivateChannelExtensions
    {
        /// <summary>
        /// Gets the account's private channels
        /// </summary>
        public static IReadOnlyList<PrivateChannel> GetPrivateChannels(this DiscordSocketClient client)
        {
            if (client.Config.Cache)
                return client.PrivateChannels;
            else
                return ((DiscordClient)client).GetPrivateChannels();
        }
    }
}
