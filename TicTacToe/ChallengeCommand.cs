using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    [SlashCommand("challenge", "Challenges a user to a game of Tic Tac Toe")]
    public class ChallengeCommand : SlashCommand
    {
        [SlashParameter("user", "The user you wish to challenge")]
        public DiscordUser Target { get; private set; }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public override InteractionResponseProperties Handle()
        {
            string id = RandomString(8);
            Program.Games[id] = new Game() { Challenger = Member.User, Challengee = Target, Id = id };

            return new InteractionResponseProperties()
            {
                Content = $"{Member.User.AsMessagable()} is challenging you, {Target.AsMessagable()}",
                Components = new List<MessageComponent>()
                {
                    new RowComponent(new List<MessageComponent>()
                    {
                        new ButtonComponent() { Style = MessageButtonStyle.Secondary, Text = "Deny", Id = $"{id}-deny" },
                        new ButtonComponent() { Style = MessageButtonStyle.Primary, Text = "Accept", Id = $"{id}-accept" }
                    })
                }
            };
        }
    }
}
