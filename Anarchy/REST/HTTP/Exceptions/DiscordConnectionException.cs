using System;

namespace Discord
{
    public class DiscordConnectionException : Exception
    {
        public DiscordConnectionException() : base("Failed to connect to Discord")
        { }
    }
}
