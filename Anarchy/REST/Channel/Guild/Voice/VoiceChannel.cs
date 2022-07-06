using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Represents a <see cref="DiscordChannel"/> specific to guild voice channels
    /// </summary>
    public class VoiceChannel : GuildChannel
    {
        [JsonProperty("bitrate")]
        public uint Bitrate { get; private set; }


        [JsonProperty("user_limit")]
        public uint UserLimit { get; private set; }


        protected void Update(VoiceChannel channel)
        {
            base.Update(channel);
            Bitrate = channel.Bitrate;
            UserLimit = channel.UserLimit;
        }


        public new async Task UpdateAsync()
        {
            Update((VoiceChannel)await Client.GetChannelAsync(Id));
        }

        /// <summary>
        /// Updates the channel's info
        /// </summary>
        public new void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }


        public async Task ModifyAsync(VoiceChannelProperties properties)
        {
            Update(await Client.ModifyGuildChannelAsync(Id, properties));
        }

        /// <summary>
        /// Modifies the channel
        /// </summary>
        /// <param name="properties">Options for modifying the channel</param>
        public void Modify(VoiceChannelProperties properties)
        {
            ModifyAsync(properties).GetAwaiter().GetResult();
        }


        public async Task<DiscordInvite> CreateInviteAsync(InviteProperties properties = null)
        {
            return await Client.CreateInviteAsync(Id, properties);
        }

        /// <summary>
        /// Creates an invite
        /// </summary>
        /// <param name="properties">Options for creating the invite</param>
        /// <returns></returns>
        public DiscordInvite CreateInvite(InviteProperties properties = null)
        {
            return CreateInviteAsync(properties).GetAwaiter().GetResult();
        }
    }
}