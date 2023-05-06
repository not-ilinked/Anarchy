using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discord
{
    /// <summary>
    /// Represents a <see cref="DiscordChannel"/> specific to groups
    /// </summary>
    public class DiscordGroup : PrivateChannel
    {
        [JsonPropertyName("icon")]
        private string _iconHash;

        public DiscordCDNImage Icon
        {
            get
            {
                if (_iconHash == null)
                    return null;
                else
                    return new DiscordCDNImage(CDNEndpoints.ChannelIcon, Id, _iconHash);
            }
        }

        [JsonPropertyName("owner_id")]
        public ulong OwnerId { get; private set; }

        protected void Update(DiscordGroup group)
        {
            base.Update(group);
            _iconHash = group._iconHash;
            OwnerId = group.OwnerId;
        }

        public new async Task UpdateAsync()
        {
            Update((DiscordGroup) await Client.GetChannelAsync(Id));
        }

        /// <summary>
        /// Updates the group's info
        /// </summary>
        public new void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }

        public async Task ModifyAsync(GroupProperties properties)
        {
            Update(await Client.ModifyGroupAsync(Id, properties));
        }

        /// <summary>
        /// Modifies the group
        /// </summary>
        /// <param name="properties">Options for modifying the group</param>
        public void Modify(GroupProperties properties)
        {
            ModifyAsync(properties).GetAwaiter().GetResult();
        }

        public async Task AddRecipientAsync(ulong userId)
        {
            await Client.AddUserToGroupAsync(Id, userId);
        }

        /// <summary>
        /// Adds a recipient to the group
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public void AddRecipient(ulong userId)
        {
            AddRecipientAsync(userId).GetAwaiter().GetResult();
        }

        public async Task RemoveRecipientAsync(ulong userId)
        {
            await Client.RemoveUserFromGroupAsync(Id, userId);
        }

        /// <summary>
        /// Removes a user from the group
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public void RemoveRecipient(ulong userId)
        {
            RemoveRecipientAsync(userId).GetAwaiter().GetResult();
        }

        public async Task<DiscordInvite> CreateInviteAsync()
        {
            return await Client.CreateInviteAsync(Id, new InviteProperties() { MaxAge = 10800 });
        }

        /// <summary>
        /// Creates an invite
        /// </summary>
        public DiscordInvite CreateInvite()
        {
            return CreateInviteAsync().GetAwaiter().GetResult();
        }
    }
}