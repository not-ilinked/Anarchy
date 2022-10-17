using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordCrosspostWebhook : DiscordWebhook
    {
        public DiscordCrosspostWebhook()
        {
            OnClientUpdated += (s, e) =>
            {
                SourceGuild.SetClient(Client);
                SourceChannel.SetClient(Client);
            };
        }

        [JsonProperty("source_guild")]
        public BaseGuild SourceGuild { get; private set; }

        [JsonProperty("source_channel")]
        public CrosspostChannel SourceChannel { get; private set; }

        private void Update(DiscordCrosspostWebhook hook)
        {
            SourceGuild = hook.SourceGuild;
            SourceChannel = hook.SourceChannel;
            base.Update(hook);
        }

        public new async Task UpdateAsync()
        {
            Update(await Client.GetWebhookAsync(Id));
        }

        /// <summary>
        /// Updates the webhook's info
        /// </summary>
        public new void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }

        public new async Task ModifyAsync(DiscordWebhookProperties properties)
        {
            Update(await Client.ModifyWebhookAsync(Id, properties));
        }

        /// <summary>
        /// Modifies the webhook
        /// </summary>
        /// <param name="properties">Options for modifying the webhook</param>
        public new void Modify(DiscordWebhookProperties properties)
        {
            ModifyAsync(properties).GetAwaiter().GetResult();
        }
    }
}
