using Discord;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class Game
    {
        public Game()
        {
            Grid = new SquareState[3][];

            for (int i = 0; i < Grid.Length; i++)
                Grid[i] = new SquareState[] { SquareState.Neutral, SquareState.Neutral, SquareState.Neutral };

            ChallengerTurn = true;
        }

        public string Id { get; set; }
        public DiscordUser Challenger { get; set; }
        public DiscordUser Challengee { get; set; }
        public bool Accepted { get; set; }

        public bool ChallengerTurn { get; set; }
        public SquareState[][] Grid { get; set; }

        private string SerializeSquare(SquareState state)
        {
            switch (state)
            {
                case SquareState.Neutral:
                    return "⚫";
                case SquareState.Challenger:
                    return "🔵";
                case SquareState.Challengee:
                    return "🔴";
                default:
                    return null; // this is impossible
            }
        }

        private bool HasWon(SquareState targetState)
        {
            foreach (var row in Grid)
            {
                if (row.All(s => s == targetState)) 
                    return true;
            }

            for (int i = 0; i < 3; i++)
            {
                if (Grid.All(row => row[i] == targetState)) 
                    return true;
            }

            if (Grid[0][0] == targetState && Grid[1][1] == targetState && Grid[2][2] == targetState) return true;
            else if (Grid[0][2] == targetState && Grid[1][1] == targetState && Grid[2][0] == targetState) return true;

            return false;
        }

        private bool TryFindWinner(out bool challengerWin)
        {
            if (HasWon(SquareState.Challenger))
            {
                challengerWin = true;
                return true;
            }
            else if (HasWon(SquareState.Challengee))
            {
                challengerWin = false;
                return true;
            }

            challengerWin = false;
            return false;
        }

        public InteractionResponseProperties SerializeGrid()
        {
            bool hasWinner = TryFindWinner(out bool challengerIsWinner);

            List<MessageComponent> rows = new List<MessageComponent>();

            for (int i = 0; i < Grid.Length; i++)
            {
                List<MessageComponent> buttons = new List<MessageComponent>();

                for (int j = 0; j < Grid[i].Length; j++)
                    buttons.Add(new ButtonComponent() { Style = MessageButtonStyle.Secondary, Id = $"{Id}-{i}-{j}", Text = SerializeSquare(Grid[i][j]), Disabled = Grid[i][j] != SquareState.Neutral || hasWinner });

                rows.Add(new RowComponent(buttons));
            }

            var embed = new EmbedMaker() { Title = "Tic Tac Toe" }
                                .AddField($"{SerializeSquare(SquareState.Challenger)} Player 1", Challenger.AsMessagable())
                                .AddField($"{SerializeSquare(SquareState.Challengee)} Player 2", Challengee.AsMessagable());

            if (hasWinner) embed.Description = $"{(challengerIsWinner ? Challenger.AsMessagable() : Challengee.AsMessagable())} won";
            else if (!Grid.Any(row => row.Any(col => col == SquareState.Neutral))) embed.Description = "The game resulted in a tie";
            else embed.Description = $"{(ChallengerTurn ? Challenger.AsMessagable() : Challengee.AsMessagable())}'s turn";

            var properties = new InteractionResponseProperties() { Components = rows, Embed = embed };

            return properties;
        }
    }
}
