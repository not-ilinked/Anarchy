using System;

namespace Discord.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public CommandAttribute(string name, string description = null)
        {
            Name = name;
            Description = description;
        }
    }
}
