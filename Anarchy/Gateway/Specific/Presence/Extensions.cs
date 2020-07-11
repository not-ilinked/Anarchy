namespace Discord.Gateway
{
    public static class PresenceExtensions
    {
        /// <summary>
        /// Updates the client's presence
        /// </summary>
        public static void UpdatePresence(this DiscordSocketClient client, PresenceChange presence)
        {
            client.Send(GatewayOpcode.PresenceChange, presence);
        }


        /// <summary>
        /// Changes the client's status (online, idle, dnd or invisible)
        /// </summary>
        /// <param name="status">The new status</param>
        public static void SetStatus(this DiscordSocketClient client, UserStatus status)
        {
            client.UpdatePresence(new PresenceChange() { Status = status });
        }


        /// <summary>
        /// Sets the client's activity
        /// </summary>
        public static void SetActivity(this DiscordSocketClient client, Activity activity)
        {
            client.UpdatePresence(new PresenceChange() { Activity = activity });
        }
    }
}