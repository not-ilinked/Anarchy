using System;

namespace Discord.Commands
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SlashParameterAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Required { get; private set; }
        public bool AutoComplete { get; private set; }

        public SlashParameterAttribute(string name, string description, bool required = false, bool autoComplete = false)
        {
            Name = name;
            Description = description;
            Required = required;
            AutoComplete = autoComplete;
        }
    }
}
