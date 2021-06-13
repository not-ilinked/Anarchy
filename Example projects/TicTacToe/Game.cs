using Discord;
using Discord.Gateway;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class Game
    {
        public Game(DiscordSocketClient client, DiscordUser challenger, DiscordUser challengee)
        {
            Client = client;
            Challenger = challenger;
            Challengee = challengee;

            Grid = new SquareState[3][];

            for (int i = 0; i < Grid.Length; i++)
                Grid[i] = new SquareState[] { SquareState.Neutral, SquareState.Neutral, SquareState.Neutral };

            ChallengerTurn = true;
        }

        public DiscordSocketClient Client { get; }
        public DiscordUser Challenger { get; }
        public DiscordUser Challengee { get; }

        public bool ChallengerTurn { get; set; }
        public SquareState[][] Grid { get; }

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

        private bool ValidMover(ulong moverId)
        {
            ulong id;

            if (ChallengerTurn) id = Challenger.Id;
            else id = Challengee.Id;

            return moverId == id;
        }

        public InteractionResponseProperties SerializeState()
        {
            bool hasWinner = TryFindWinner(out bool challengerIsWinner);

            var form = new DiscordComponentForm(Client);

            for (int i = 0; i < Grid.Length; i++)
            {
                List<ComponentFormButton> buttons = new List<ComponentFormButton>();

                for (int j = 0; j < Grid[i].Length; j++)
                {
                    int y = i;
                    int x = j;

                    var square = new ComponentFormButton(MessageButtonStyle.Secondary, SerializeSquare(Grid[y][x])) { Disabled = Grid[y][x] != SquareState.Neutral || hasWinner };
                    square.OnClick += (s, e) =>
                    {
                        if (ValidMover(e.Member.User.Id))
                        {
                            Grid[y][x] = ChallengerTurn ? SquareState.Challenger : SquareState.Challengee;
                            ChallengerTurn = !ChallengerTurn;
                            e.Respond(InteractionCallbackType.UpdateMessage, SerializeState());
                        }
                    };

                    buttons.Add(square);
                }

                form.Rows.Add(buttons);
            }

            var embed = new EmbedMaker() { Title = "Tic Tac Toe" }
                                .AddField($"{SerializeSquare(SquareState.Challenger)} Player 1", Challenger.AsMessagable())
                                .AddField($"{SerializeSquare(SquareState.Challengee)} Player 2", Challengee.AsMessagable());

            if (hasWinner) embed.Description = $"{(challengerIsWinner ? Challenger.AsMessagable() : Challengee.AsMessagable())} won";
            else if (!Grid.Any(row => row.Any(col => col == SquareState.Neutral))) embed.Description = "The game resulted in a tie";
            else embed.Description = $"{(ChallengerTurn ? Challenger.AsMessagable() : Challengee.AsMessagable())}'s turn";

            return new InteractionResponseProperties() { Content = null, Components = form, Embed = embed };
        }
    }
}
