using System;
using Discord;
using Discord.Commands;

namespace TicTacToe
{
    [SlashCommand("ping", "Ping bot")]
    public class PingCommand : SlashCommand
    {
        public override InteractionResponseProperties Handle()
        {
            Interaction.Respond(InteractionCallbackType.RespondWithMessage, new InteractionResponseProperties() { Content = $"🏓 Pong !\nWebsocket Latency: {Client.ping}ms", Ephemeral = false });

            return null;
        }
    }
}
