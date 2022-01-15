using System;
using System.Collections.Generic;
using System.Reflection;

namespace Discord.Commands
{
    public class DiscordCommand : CommandAttribute
    {
        public DiscordCommand(Type type, CommandAttribute attr) : base(attr.Name, attr.Description)
        {
            List<CommandParameter> parameters = new List<CommandParameter>();
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (CommandHandler.TryGetAttribute(property.GetCustomAttributes(), out ParameterAttribute pAttr))
                {
                    parameters.Add(new CommandParameter(pAttr, property));
                }
            }
            Parameters = parameters;
            Type = type;
        }

        public Type Type { get; private set; }
        public IReadOnlyList<CommandParameter> Parameters { get; private set; }
    }
}