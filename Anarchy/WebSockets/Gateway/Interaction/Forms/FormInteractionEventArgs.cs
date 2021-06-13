using System;
using System.Threading.Tasks;

namespace Discord.Gateway
{
    public class FormInteractionEventArgs : EventArgs
    {
        private readonly DiscordSocketClient _client;
        private readonly ulong _id;
        private readonly ulong _appId;
        private readonly string _token;

        internal FormInteractionEventArgs(DiscordSocketClient client, DiscordInteraction interaction)
        {
            _client = client;
            _id = interaction.Id;
            _appId = interaction.ApplicationId;
            _token = interaction.Token;

            Member = interaction.Member;
            User = interaction.User;
        }

        public GuildMember Member { get; }
        public DiscordUser User { get; }

        public Task RespondAsync(InteractionCallbackType callbackType, InteractionResponseProperties properties = null) => _client.RespondToInteractionAsync(_id, _token, callbackType, properties);
        public void Respond(InteractionCallbackType callbackType, InteractionResponseProperties properties = null) => RespondAsync(callbackType, properties).GetAwaiter().GetResult();

        public Task ModifyResponseAsync(InteractionResponseProperties changes) => _client.ModifyInteractionResponseAsync(_appId, _token, changes);
        public void ModifyResponse(InteractionResponseProperties changes) => ModifyResponseAsync(changes).GetAwaiter().GetResult();
    }
}
