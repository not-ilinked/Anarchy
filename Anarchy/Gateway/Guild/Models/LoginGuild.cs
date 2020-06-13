namespace Discord.Gateway
{
    /// <summary>
    /// Only used when the client has logged in.
    /// If you're logging into a user account u can call ToSocketGuild() which will return the full guild.
    /// If you are on a bot account however, please pay attention to the OnJoinedGuild events, which will be dispatched as guilds become available to the bot.
    /// </summary>
    public class LoginGuild : MinimalGuild
    {
        /// <summary>
        /// Gets the full guild.
        /// Please only use this method if the account type is User
        /// </summary>
        public SocketGuild ToSocketGuild()
        {
            return Json.ToObject<SocketGuild>().SetJson(Json).SetClient(Client);
        }
    }
}
