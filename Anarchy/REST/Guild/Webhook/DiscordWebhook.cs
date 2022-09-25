using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordWebhook : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        [JsonProperty("type")]
        public DiscordWebhookType Type { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("avatar")]
        private string _avatarHash;

        public DiscordCDNImage Avatar
        {
            get
            {
                if (_avatarHash == null)
                    return null;
                else
                    return new DiscordCDNImage(CDNEndpoints.Avatar, Id, _avatarHash);
            }
        }

        [JsonProperty("user")]
        public DiscordUser Creator { get; private set; }

        [JsonProperty("channel_id")]
        public ulong ChannelId { get; private set; }

        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }

        public DiscordWebhook()
        {
            OnClientUpdated += (s, e) => Creator.SetClient(Client);
        }

        protected void Update(DiscordWebhook hook)
        {
            Id = hook.Id;
            Type = hook.Type;
            Name = hook.Name;
            _avatarHash = hook._avatarHash;
            Creator = hook.Creator;
            ChannelId = hook.ChannelId;
            GuildId = hook.GuildId;
        }

        public async Task UpdateAsync()
        {
            Update(await Client.GetWebhookAsync(Id));
        }

        /// <summary>
        /// Updates the webhook's info
        /// </summary>
        public void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }

        public async Task ModifyAsync(DiscordWebhookProperties properties)
        {
            Update(await Client.ModifyWebhookAsync(Id, properties));
        }

        /// <summary>
        /// Modifies the webhook
        /// </summary>
        /// <param name="properties">Options for modifying the webhook</param>
        public void Modify(DiscordWebhookProperties properties)
        {
            ModifyAsync(properties).GetAwaiter().GetResult();
        }

        public async Task DeleteAsync()
        {
            await Client.DeleteWebhookAsync(Id);
        }

        /// <summary>
        /// Deletes the webhook
        /// </summary>
        public void Delete()
        {
            DeleteAsync().GetAwaiter().GetResult();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
