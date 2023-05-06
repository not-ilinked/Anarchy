using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class DiscordInteractionData : Controllable
    {
        public DiscordInteractionData()
        {
            OnClientUpdated += (s, e) => Resolved.SetClient(Client);
        }

        [JsonPropertyName("id")]
        public ulong CommandId { get; private set; }

        [JsonPropertyName("name")]
        public string CommandName { get; private set; }

        [JsonPropertyName("options")]
        public IReadOnlyList<SlashCommandArgument> CommandArguments { get; private set; }

        [JsonPropertyName("resolved")]
        public ResolvedInteractionData Resolved { get; private set; }

        [JsonPropertyName("custom_id")]
        public string ComponentId { get; private set; }

        [JsonPropertyName("component_type")]
        public MessageComponentType ComponentType { get; private set; }

        [JsonPropertyName("values")]
        public string[] SelectMenuValues { get; private set; }
    }
}
