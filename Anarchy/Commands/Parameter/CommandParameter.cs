using System.Reflection;

namespace Discord.Commands
{
    public class CommandParameter : ParameterAttribute
    {
        public CommandParameter(ParameterAttribute attr, PropertyInfo property) : base(attr.Name, attr.Optional)
        {
            Property = property;
        }

        public PropertyInfo Property { get; private set; }
    }
}
