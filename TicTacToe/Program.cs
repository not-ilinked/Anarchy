using Discord;
using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TicTacToe
{
    class Program
    {
        public static Dictionary<string, Game> Games = new Dictionary<string, Game>();

        static void Main(string[] args)
        {
            string token = "ODUwNDA0NDExMTE2NzQ4ODIx.YLpO0w.CcVOS0NdLNeyYA_cVm6RCmURV0E";

            DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig() { Intents = DiscordGatewayIntent.Guilds });
            client.OnLoggedIn += Client_OnLoggedIn;
            client.OnInteraction += Client_OnInteraction;
            client.OnLoggedOut += Client_OnLoggedOut;
            client.Login("Bot " + token);

            Thread.Sleep(-1);
        }

        private static bool ValidMover(Game game, ulong moverId)
        {
            ulong id;

            if (game.ChallengerTurn) id = game.Challenger.Id;
            else id = game.Challengee.Id;

            return moverId == id;
        }

        private static void Client_OnInteraction(DiscordSocketClient client, DiscordInteractionEventArgs args)
        {
            if (args.Interaction.Type == DiscordInteractionType.MessageComponent)
            {
                string[] parts = args.Interaction.Data.ComponentId.Split('-');

                var game = Games[parts[0]];
                if (game.Accepted)
                {
                    if (ValidMover(game, args.Interaction.Member.User.Id))
                    {
                        game.Grid[int.Parse(parts[1])][int.Parse(parts[2])] = game.ChallengerTurn ? SquareState.Challenger : SquareState.Challengee;
                        game.ChallengerTurn = !game.ChallengerTurn;
                        args.Interaction.Respond(InteractionCallbackType.UpdateMessage, game.UpdateGrid());

                        return;
                    }
                }
                else if (args.Interaction.Member.User.Id == game.Challengee.Id)
                {
                    if (parts[1] == "accept")
                    {
                        game.Accepted = true;
                        args.Interaction.Respond(InteractionCallbackType.UpdateMessage, game.UpdateGrid());
                    }
                    else
                    {
                        args.Interaction.Respond(InteractionCallbackType.UpdateMessage, new InteractionResponseProperties() { Components = new List<MessageComponent>(), Content = $"{game.Challengee.AsMessagable()} declined {game.Challenger.AsMessagable()}'s challenge" });
                        Games.Remove(parts[0]);
                    }

                    return;
                }
                
                args.Interaction.Respond(InteractionCallbackType.UpdateMessage, new InteractionResponseProperties());
            }
        }

        private static void Client_OnLoggedOut(DiscordSocketClient client, LogoutEventArgs args)
        {
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine("Logged in");

            client.RegisterSlashCommands();
        }
    }
}
