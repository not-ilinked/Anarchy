using System;

namespace Discord
{
    public class DiscordNitro
    {
        public DiscordNitroType Type { get; private set; }
        public DateTime? Since { get; private set; }


        public DiscordNitro(string since)
        {
            if (since != null)
            {
                Type = DiscordNitroType.Unknown;
                Since = DiscordTimestamp.FromString(since);
            }
        }


        public override string ToString()
        {
            return Since.ToString();
        }
    }
}
