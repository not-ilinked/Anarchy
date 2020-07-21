namespace Discord.Gateway
{
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
