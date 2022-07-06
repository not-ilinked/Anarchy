﻿using System.Collections.Generic;
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
        public ulong CommandId { get; private set; }

        [JsonProperty("name")]
        public string CommandName { get; private set; }

        [JsonProperty("options")]
        public IReadOnlyList<SlashCommandArgument> CommandArguments { get; private set; }

        [JsonProperty("resolved")]
        public ResolvedInteractionData Resolved { get; private set; }

        [JsonProperty("custom_id")]
        public string ComponentId { get; private set; }

        [JsonProperty("component_type")]
        public MessageComponentType ComponentType { get; private set; }

        [JsonProperty("values")]
        public string[] SelectMenuValues { get; private set; }
    }
}
