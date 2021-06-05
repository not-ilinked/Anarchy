using System;

namespace Discord.Commands
{
    public class SlashParameterAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Required { get; private set; }

        public SlashParameterAttribute(string name, string description, bool required = true)
        {
            Name = name;
            Description = description;
            Required = required;
        }
    }
}
