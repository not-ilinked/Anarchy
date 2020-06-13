using System;

namespace Discord
{
    public class DiscordException : Exception
    {
        public DiscordClient Client { get; private set; }

        internal DiscordException(DiscordClient client, string message = null) : base(message)
        {
            Client = client;
        }
    }
}