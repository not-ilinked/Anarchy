using System;

namespace Discord.Gateway
{
    public class UserTypingEventArgs : EventArgs
    {
        public UserTyping Typing { get; private set; }

        internal UserTypingEventArgs(UserTyping typing)
        {
            Typing = typing;
        }

        public override string ToString()
        {
            return Typing.ToString();
        }
    }
}
