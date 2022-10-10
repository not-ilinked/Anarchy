using System.Threading.Tasks;
using Discord.Gateway;

namespace Discord
{
    public static class InteractionExtensions
    {
        public static Task RespondToInteractionAsync(this DiscordClient client, ulong interactionId, string interactionToken, InteractionCallbackType callbackType, InteractionResponseProperties properties = null) =>
            client.HttpClient.PostAsync($"/interactions/{interactionId}/{interactionToken}/callback", new InteractionResponse() { Type = callbackType, Data = properties });

        public static void RespondToInteraction(this DiscordClient client, ulong interactionId, string interactionToken, InteractionCallbackType callbackType, InteractionResponseProperties properties = null) => client.RespondToInteractionAsync(interactionId, interactionToken, callbackType, properties);

        public static Task ModifyInteractionResponseAsync(this DiscordClient client, ulong appId, string interactionToken, InteractionResponseProperties changes) =>
            client.HttpClient.PatchAsync($"/webhooks/{appId}/{interactionToken}/messages/@original", changes);

        public static DiscordInteraction GetRespondInteraction(this DiscordClient client, ulong appId, string interactionToken) => 
            client.HttpClient.GetAsync($"/webhooks/{appId}/{interactionToken}/messages/@original").GetAwaiter().GetResult().Deserialize<DiscordInteraction>();

        public static Task SendInteractionAsync(this DiscordClient client, DiscordInteraction sPostData) =>
            client.HttpClient.PostAsync($"/interactions", sPostData);
    }
}
