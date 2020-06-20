using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Represents a <see cref="DiscordChannel"/> specific to groups
    /// </summary>
    public class Group : PrivateChannel
    {
        [JsonProperty("icon")]
        private string _iconHash;

        public DiscordChannelIcon Icon
        {
            get
            {
                return new DiscordChannelIcon(Id, _iconHash);
            }
        }


        [JsonProperty("owner_id")]
        public ulong OwnerId { get; private set; }


        protected void Update(Group group)
        {
            base.Update(group);
            _iconHash = group._iconHash;
            OwnerId = group.OwnerId;
        }


        /// <summary>
        /// Updates the group's info
        /// </summary>
        public new void Update()
        {
            Update(Client.GetChannel(Id).ToGroup());
        }


        /// <summary>
        /// Modifies the group
        /// </summary>
        /// <param name="properties">Options for modifying the group</param>
        public void Modify(GroupProperties properties)
        {
            Update(Client.ModifyGroup(Id, properties));
        }


        /// <summary>
        /// Adds a recipient to the group
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public void AddRecipient(ulong userId)
        {
            Client.AddUserToGroup(Id, userId);
        }


        /// <summary>
        /// Removes a user from the group
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public void RemoveRecipient(ulong userId)
        {
            Client.RemoveUserFromGroup(Id, userId);
        }


        /// <summary>
        /// Removes a user from the group
        /// </summary>
        public void RemoveRecipient(DiscordUser user)
        {
            RemoveRecipient(user.Id);
        }


        /// <summary>
        /// Creates an invite
        /// </summary>
        public DiscordInvite CreateInvite()
        {
            return Client.CreateInvite(Id, new InviteProperties() { MaxAge = 10800 });
        }
    }
}