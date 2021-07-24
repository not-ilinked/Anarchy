using System;

namespace Discord.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SlashCommandCategoryAttribute : Attribute
    {
        public string Category { get; }

        public SlashCommandCategoryAttribute(string categoryName)
        {
            Category = categoryName;
        }
    }
}
