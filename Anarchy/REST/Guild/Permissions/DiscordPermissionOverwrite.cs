using Newtonsoft.Json;

namespace Discord
{
    public class DiscordPermissionOverwrite
    {
        [JsonProperty("id")]
        public ulong AffectedId { get; internal set; }

        [JsonProperty("type")]
        public PermissionOverwriteType Type { get; internal set; }

        [JsonProperty("allow")]
        public DiscordPermission Allow { get; internal set; }

        [JsonProperty("deny")]
        public DiscordPermission Deny { get; internal set; }

        public OverwrittenPermissionState GetPermissionState(DiscordPermission permission)
        {
            if (Allow.HasFlag(permission))
                return OverwrittenPermissionState.Allow;
            else if (Deny.HasFlag(permission))
                return OverwrittenPermissionState.Deny;
            else
                return OverwrittenPermissionState.Inherit;
        }
    }
}
