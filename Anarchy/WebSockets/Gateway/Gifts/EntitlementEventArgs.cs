using System;

namespace Discord.Gateway
{
    public class EntitlementEventArgs : EventArgs
    {
        public DiscordEntitlement Entitlement { get; private set; }

        public EntitlementEventArgs(DiscordEntitlement entitlement)
        {
            Entitlement = entitlement;
        }
    }
}
