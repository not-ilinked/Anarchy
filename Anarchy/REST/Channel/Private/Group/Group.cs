using System.Drawing;
using System.Net.Http;
using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Represents a <see cref="DiscordChannel"/> specific to groups
    /// </summary>
    public class Group : PrivateChannel
    {
        [JsonProperty("icon")]
        public string IconId { get; private set; }


        [JsonProperty("owner_id")]
        public ulong OwnerId { get; private set; }


        protected void Update(Group group)
        {
            base.Update(group);
            IconId = group.IconId;
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
        /// Adds a recipient to the group
        /// </summary>
        public void AddRecipient(DiscordUser user)
        {
            AddRecipient(user.Id);
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


        public new void Leave()
        {
            Group group = Client.DeleteChannel(Id).ToGroup();

            Update(group);
        }


        /// <summary>
        /// Creates an invite
        /// </summary>
        /// <param name="properties">Options for creating the invite</param>
        public DiscordInvite CreateInvite(InviteProperties properties = null)
        {
            return Client.CreateInvite(Id, properties);
        }


        /// <summary>
        /// Gets the group's icon
        /// </summary>
        /// <returns>The icon (null if IconId is null)</returns>
        public Image GetIcon()
        {
            if (IconId == null)
                return null;

            return (Bitmap)new ImageConverter()
                        .ConvertFrom(new HttpClient().GetByteArrayAsync($"https://cdn.discordapp.com/icons/{Id}/{IconId}.png").Result);
        }
    }
}