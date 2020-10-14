using System.Collections.Generic;

namespace Discord.Gateway
{
    public class LoginEventArgs
    {
        public DiscordClientUser User { get; private set; }
        public IReadOnlyList<MinimalGuild> Guilds { get; private set; }
        public IReadOnlyList<PrivateChannel> PrivateChannels { get; private set; }
        public IReadOnlyList<DiscordRelationship> Relationships { get; private set; }
        public IReadOnlyList<ClientConnectedAccount> ConnectedAccounts { get; private set; }
        public IReadOnlyList<DiscordPresence> Presences { get; private set; }

        internal LoginEventArgs(Login login)
        {
            User = login.User;
            Guilds = login.Guilds;
            PrivateChannels = login.PrivateChannels;
            Relationships = login.Relationships;
            ConnectedAccounts = login.ConnectedAccounts;
            Presences = login.Presences;
        }


        public override string ToString()
        {
            return User.ToString();
        }
    }
}
