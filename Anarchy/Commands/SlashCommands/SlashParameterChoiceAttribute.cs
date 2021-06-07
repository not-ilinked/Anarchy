using System;
using System.Collections.Generic;

namespace Discord.Commands
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SlashParameterChoiceAttribute : Attribute
    {
        public string Name { get; private set; }
        public object Value { get; private set; }

        public SlashParameterChoiceAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
