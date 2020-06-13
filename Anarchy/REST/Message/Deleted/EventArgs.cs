using System;

namespace Discord
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
