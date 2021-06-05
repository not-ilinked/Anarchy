using System;

namespace Discord.Commands
{
    public class SlashCommandAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Delayed { get; private set; }

        public SlashCommandAttribute(string name, string description, bool delayed = false)
        {
            Name = name;
            Description = description;
            Delayed = delayed;
        }
    }
}
