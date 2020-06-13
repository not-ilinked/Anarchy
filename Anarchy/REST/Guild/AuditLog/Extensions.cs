using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Discord
{
    public static class AuditLogExtensions
    {
        /// <summary>
        /// Gets the audit log for the specified guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <returns>A <see cref="IReadOnlyList{AuditLogEntry}"/></returns>
        public static IReadOnlyList<AuditLogEntry> GetAuditLog(this DiscordClient client, ulong guildId, AuditLogFilters filters = null)
        {
            if (filters == null)
                filters = new AuditLogFilters();

            return client.HttpClient.Get($"/guilds/{guildId}/audit-logs?{(filters.UserIdProperty.Set ? $"user_id={filters.UserId}" : "")}&{(filters.ActionTypeProperty.Set ? $"action_type={(int)filters.ActionType}" : "")}&{(filters.BeforeIdProperty.Set ? $"before={filters.BeforeId}" : "")}&{(filters.LimitProperty.Set ? $"limit={filters.Limit}" : "")}")
                                .Deserialize<JObject>().Value<IReadOnlyList<AuditLogEntry>>("audit_log_entries");
        }
    }
}