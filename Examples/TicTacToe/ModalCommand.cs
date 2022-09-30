using System;
using System.Collections.Generic;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;

namespace TicTacToe
{
    [SlashCommand("modal", "Show a modal")]
    public class ModalCommand : SlashCommand
    {
        [ModalParameter("name")]
        public string Name { get; private set; }

        [ModalParameter("detail", "Abc", false, TextInputStyle.Paragraph)]
        public string Detail { get; private set; }

        public override InteractionResponseProperties Handle()
        {
            ShowModal("My Cool Modal");            

            return null;
        }

        public override InteractionResponseProperties HandleModalSubmit()
        {
            //Get submit data
            Console.WriteLine(Name);
            Console.WriteLine(Detail);
            return new InteractionResponseProperties() { Content = "Thank you for submit modal !", Ephemeral = true };
        }

    }
}
