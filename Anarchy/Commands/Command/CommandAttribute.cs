using System;

namespace Discord.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string Command { get; private set; }
        public string Description { get; private set; }

        public CommandAttribute(string cmd, string description = null)
        {
            Command = cmd;
            Description = description;
        }
    }
}
