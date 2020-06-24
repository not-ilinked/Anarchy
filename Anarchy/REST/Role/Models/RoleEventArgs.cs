namespace Discord
{
    public class RoleEventArgs
    {
        public DiscordRole Role { get; private set; }

        internal RoleEventArgs(DiscordRole role)
        {
            Role = role;
        }


        public override string ToString()
        {
            return Role.ToString();
        }
    }
}
