using System;
using System.Collections.Generic;

namespace Discord
{
    public class UserListEventArgs : EventArgs
    {
        public IReadOnlyList<DiscordUser> Users { get; private set; }

        internal UserListEventArgs(List<DiscordUser> users)
        {
            Users = users;
        }
    }
}