using Discord;
using Discord.Commands;

namespace TicTacToe
{
    [SlashCommand("edit", "Ping bot")]
    [SlashCommandCategory("fun")]
    public class FunEditCommand : SlashCommand
    {
        public override InteractionResponseProperties Handle()
        {
            return new InteractionResponseProperties() { Content = $"🏓 Pong Edit !", Ephemeral = true };
        }
    }
}
