namespace Discord.Gateway
{
    public class PresenceUpdatedEventArgs
    {
        public DiscordPresence Presence { get; private set; }

        internal PresenceUpdatedEventArgs(DiscordPresence presence)
        {
            Presence = presence;
        }


        public override string ToString()
        {
            return Presence.ToString();
        }
    }
}
