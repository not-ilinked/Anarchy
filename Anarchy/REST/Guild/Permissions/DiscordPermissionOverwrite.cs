

using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordPermissionOverwrite
    {
        [JsonPropertyName("id")]
        public ulong AffectedId { get; internal set; }

        [JsonPropertyName("type")]
        public PermissionOverwriteType Type { get; internal set; }

        [JsonPropertyName("allow")]
        public DiscordPermission Allow { get; internal set; }

        [JsonPropertyName("deny")]
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
