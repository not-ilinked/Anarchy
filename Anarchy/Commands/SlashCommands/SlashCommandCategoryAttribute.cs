using System;

namespace Discord.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SlashCommandCategoryAttribute : Attribute
    {
        public string Category { get; }
        public string SubcommandGroup { get; }

        public SlashCommandCategoryAttribute(string categoryName, string subcommandGroup = null)
        {
            Category = categoryName;
            SubcommandGroup = subcommandGroup;
        }
    }
}
