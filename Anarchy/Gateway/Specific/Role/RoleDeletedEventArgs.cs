using System;

namespace Discord.Gateway
{
    public class RoleDeletedEventArgs : EventArgs
    {
        public DeletedRole Role { get; private set; }

        internal RoleDeletedEventArgs(DeletedRole role)
        {
            Role = role;
        }
    }
}
