using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Gateway
{
    public static class GatewayIntentCalculator
    {
        public static DiscordGatewayIntent All()
        {
            DiscordGatewayIntent intents = 0;

            foreach (var intent in Enum.GetValues(typeof(DiscordGatewayIntent)))
                intents = Add(intents, (DiscordGatewayIntent)intent);

            return intents;
        }


        public static DiscordGatewayIntent Add(DiscordGatewayIntent intents, DiscordGatewayIntent intent)
        {
            return (DiscordGatewayIntent)((int)intents + (int)intent);
        }


        public static DiscordGatewayIntent Remove(DiscordGatewayIntent intents, DiscordGatewayIntent intent)
        {
            return (DiscordGatewayIntent)((int)intents - (int)intent);
        }


        public static bool Has(DiscordGatewayIntent intents, DiscordGatewayIntent comparison)
        {
            return (intents & comparison) == comparison;
        }
    }
}
