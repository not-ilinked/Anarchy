using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public static class VoiceExtensions
    {
        public static async Task RingAsync(this DiscordClient client, ulong channelId, List<ulong> recipients)
        {
            var payload = new
            {
                recipients = recipients == null ? null : recipients.ToArray()
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            await client.HttpClient.PostAsync($"/channels/{channelId}/call/ring", requestContent);
        }

        /// <summary>
        /// Rings the specified recipients
        /// </summary>
        public static void Ring(this DiscordClient client, ulong channelId, List<ulong> recipients)
        {
            client.RingAsync(channelId, recipients).GetAwaiter().GetResult();
        }

        public static async Task StartCallAsync(this DiscordClient client, ulong channelId)
        {
            await client.RingAsync(channelId, null);
        }

        /// <summary>
        /// Opens a call on the specified channel
        /// </summary>
        public static void StartCall(this DiscordClient client, ulong channelId)
        {
            client.StartCallAsync(channelId).GetAwaiter().GetResult();
        }

        public static async Task StopRingingAsync(this DiscordClient client, ulong channelId, List<ulong> recipients)
        {

            var payload = new
            {
                recipients = recipients == null ? null : recipients.ToArray()
            };
            var requestContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            await client.HttpClient.PostAsync($"/channels/{channelId}/call/stop-ringing", requestContent);
        }

        /// <summary>
        /// Stops ringing the specified recipients
        /// </summary>
        public static void StopRinging(this DiscordClient client, ulong channelId, List<ulong> recipients)
        {
            client.StopRingingAsync(channelId, recipients).GetAwaiter().GetResult();
        }

        public static async Task DeclineCallAsync(this DiscordClient client, ulong channelId)
        {
            await client.StopRingingAsync(channelId, null);
        }

        /// <summary>
        /// Declines the current incoming call from the specified channel
        /// </summary>
        public static void DeclineCall(this DiscordClient client, ulong channelId)
        {
            client.DeclineCallAsync(channelId).GetAwaiter().GetResult();
        }

        public static async Task<IReadOnlyList<VoiceRegion>> GetVoiceRegionsAsync(this DiscordClient client)
        {
            return (await client.HttpClient.GetAsync("/voice/regions"))
                                .Deserialize<IReadOnlyList<VoiceRegion>>();
        }

        /// <summary>
        /// Gets all available voice regions
        /// </summary>
        public static IReadOnlyList<VoiceRegion> GetVoiceRegions(this DiscordClient client)
        {
            return client.GetVoiceRegionsAsync().GetAwaiter().GetResult();
        }
    }
}
