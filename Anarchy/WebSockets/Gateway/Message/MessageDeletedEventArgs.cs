using System;

namespace Discord.Gateway
{
    public class MessageDeletedEventArgs : EventArgs
    {
        public DeletedMessage DeletedMessage { get; private set; }

        internal MessageDeletedEventArgs(DeletedMessage msg)
        {
            DeletedMessage = msg;
        }
    }
}
