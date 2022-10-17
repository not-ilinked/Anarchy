using System.Collections.Generic;

namespace Discord.Gateway
{
    public static class PresenceExtensions
    {
        public static DiscordPresence GetPresence(this DiscordSocketClient client, ulong userId)
        {
            try
            {
                return client.Presences[userId];
            }
            catch (KeyNotFoundException)
            {
                throw new DiscordHttpException(new DiscordHttpError(DiscordError.UnknownUser, "User was not found in cache"));
            }
        }

        /// <summary>
        /// Updates the client's presence
        /// </summary>
        public static void UpdatePresence(this DiscordSocketClient client, PresenceProperties presence)
        {
            client.Send(GatewayOpcode.PresenceChange, presence);
        }

        /// <summary>
        /// Changes the client's status (online, idle, dnd or invisible)
        /// </summary>
        /// <param name="status">The new status</param>
        public static void SetStatus(this DiscordSocketClient client, UserStatus status)
        {
            client.UpdatePresence(new PresenceProperties() { Status = status });
        }

        /// <summary>
        /// Sets the client's activity
        /// </summary>
        public static void SetActivity(this DiscordSocketClient client, ActivityProperties activity)
        {
            client.UpdatePresence(new PresenceProperties() { Activity = activity });
        }
    }
}