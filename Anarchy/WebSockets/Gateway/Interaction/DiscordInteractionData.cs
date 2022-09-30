using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class DiscordInteractionData : Controllable
    {
        public DiscordInteractionData()
        {
            OnClientUpdated += (s, e) => Resolved.SetClient(Client);
        }

        [JsonProperty("id")]
        public string CommandId { get; set; }

        [JsonProperty("name")]
        public string CommandName { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("options")]
        public List<SlashCommandArgument> CommandArguments { get; set; }

        [JsonProperty("resolved")]
        public ResolvedInteractionData Resolved { get; set; }

        [JsonProperty("components")]
        [JsonConverter(typeof(DeepJsonConverter<MessageComponent>))]
        public List<MessageComponent> Components { get; set; }

        [JsonProperty("custom_id")]
        public string ComponentId { get; set; }

        [JsonProperty("component_type")]
        public MessageComponentType ComponentType { get; set; }

        [JsonProperty("values")]
        public string[] SelectMenuValues { get; set; }
    }
}
