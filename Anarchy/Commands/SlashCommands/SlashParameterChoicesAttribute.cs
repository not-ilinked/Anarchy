using System;
using System.Collections.Generic;

namespace Discord.Commands
{
    public class SlashParameterChoicesAttribute : Attribute
    {
        public Dictionary<string, string> Choices { get; private set; }

        public SlashParameterChoicesAttribute(Dictionary<string, string> choices)
        {
            Choices = choices;
        }
    }
}
