using System;
using System.Collections.Generic;
using System.Reflection;

namespace Discord.Commands
{
    public class DiscordCommand
    {
        public DiscordCommand(string command, List<KeyValuePair<PropertyInfo, ParameterAttribute>> parameters, string description)
        {
            Command = command;

            if (parameters == null)
                parameters = new List<KeyValuePair<PropertyInfo, ParameterAttribute>>();

            _parameters = parameters;

            Description = description;
        }

        public string Command { get; private set; }
        internal List<KeyValuePair<PropertyInfo, ParameterAttribute>> _parameters { get; private set; }
        public IReadOnlyList<ParameterAttribute> Parameters
        {
            get
            {
                return _parameters.ConvertAll(p => p.Value);
            }
        }
        public string Description { get; private set; }


        public override string ToString()
        {
            string parameters = "";

            foreach (var param in Parameters)
            {
                if (param.Optional)
                    parameters += $" <{param.Name}>";
                else
                    parameters += $" [{param.Name}]";
            }

            return $"{Command}{parameters}";
        }
    }
}
