using System;
using System.Collections.Generic;
using Discord;
using Discord.Commands;

namespace TicTacToe
{
    [SlashCommand("add", "Command for fun test subcommand group")]
    [SlashCommandCategory("fun", "test")]
    public class FunTestAddCommand : SlashCommand
    {
        [SlashParameter("name", "Name", true, true)]
        public string Name { get; private set; }

        [SlashParameter("name2", "Name2", false, true)]
        public string Name2 { get; private set; }

        public override InteractionResponseProperties Handle()
        {
            if (Name.Trim() == "a") return new InteractionResponseProperties() { Content = "Fun Added", Ephemeral = true };
            return new InteractionResponseProperties() { Content = "Fun Deleted", Ephemeral = true };
        }

        public override InteractionResponseProperties HandleAutoComplete()
        {
            var forcusedOpt = GetFocusedOption();

            List<string> choices = new()
            {
                "aaa",
                "BB 1",
                "a2",
                "b22",
                "c"
            };
            if(forcusedOpt.Name == "name2")
            {
                choices = new()
                {
                    "2aaa",
                    "1BB 1",
                    "ba2",
                    "jb22",
                    "wc"
                };
            }

            ShowAutocompleteChoices(forcusedOpt.Value, choices);

            return null;
        }
    }
}
