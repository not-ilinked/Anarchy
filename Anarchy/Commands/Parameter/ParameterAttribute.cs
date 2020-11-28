using System;

namespace Discord.Commands
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterAttribute : Attribute
    {
        public string Name { get; private set; }
        public bool Optional { get; private set; }

        public ParameterAttribute(string name, bool optional = false)
        {
            Name = name;
            Optional = optional;
        }
    }
}
