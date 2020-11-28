using System;

namespace Discord.Gateway
{
    public class MessageEventArgs : EventArgs
    {
        public DiscordMessage Message { get; private set; }

        internal MessageEventArgs(DiscordMessage msg)
        {
            Message = msg;
        }


        public override string ToString()
        {
            return Message.ToString();
        }
    }
}