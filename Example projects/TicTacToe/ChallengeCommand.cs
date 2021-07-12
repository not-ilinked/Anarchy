using Discord;
using Discord.Gateway;
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
            if (Target.Id == Caller.Id) return new InteractionResponseProperties() { Content = "You cannot challenge yourself", Ephemeral = true };

            var game = new Game(Client, CallerMember.User, Target);
            
            var deny = new ComponentFormButton(MessageButtonStyle.Secondary, "Deny");
            deny.OnClick += (s, e) =>
            {
                if (e.Member.User.Id == Target.Id)
                {
                    e.Respond(InteractionCallbackType.UpdateMessage, new InteractionResponseProperties() 
                    {
                        Components = new List<MessageComponent>(), 
                        Content = $"{Target.AsMessagable()} declined {CallerMember.User.AsMessagable()}'s challenge" 
                    });
                }
            };

            var accept = new ComponentFormButton(MessageButtonStyle.Primary, "Accept");
            accept.OnClick += (s, e) =>
            {
                if (e.Member.User.Id == Target.Id)
                    e.Respond(InteractionCallbackType.UpdateMessage, game.SerializeState());
            };

            return new InteractionResponseProperties()
            {
                Content = $"{CallerMember.User.AsMessagable()} is challenging you, {Target.AsMessagable()}",
                Components = new DiscordComponentForm(Client, new List<List<ComponentFormButton>>()
                {
                    new List<ComponentFormButton>()
                    {
                        deny,
                        accept
                    }
                })
            };
        }
    }
}
