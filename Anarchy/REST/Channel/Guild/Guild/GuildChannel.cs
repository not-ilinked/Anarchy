using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discord
{
    /// <summary>
    /// Represents a <see cref="DiscordChannel"/> specific to any guild channel
    /// </summary>
    public class GuildChannel : DiscordChannel
    {
        [JsonProperty("guild_id")]
        internal ulong GuildId { get; set; }

        public MinimalGuild Guild
        {
            get { return new MinimalGuild(GuildId).SetClient(Client); }
        }


        [JsonProperty("position")]
        public uint Position { get; protected set; }
        

        [JsonProperty("parent_id")]
        public ulong? ParentId { get; protected set; }


        [JsonProperty("permission_overwrites")]
        public IReadOnlyList<DiscordPermissionOverwrite> PermissionOverwrites { get; protected set; }


        protected void Update(GuildChannel channel)
        {
            base.Update(channel);
            Position = channel.Position;
            ParentId = channel.ParentId;
            PermissionOverwrites = channel.PermissionOverwrites;
        }


        public new async Task UpdateAsync()
        {
            Update((await Client.GetChannelAsync(Id)).ToGuildChannel());
        }

        /// <summary>
        /// Updates the channel
        /// </summary>
        public new void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }


        public async Task ModifyAsync(GuildChannelProperties properties)
        {
            Update(await Client.ModifyGuildChannelAsync(Id, properties));
        }

        /// <summary>
        /// Modifies the channel
        /// </summary>
        /// <param name="properties">Options for modifying the channel</param>
        public void Modify(GuildChannelProperties properties)
        {
            ModifyAsync(properties).GetAwaiter().GetResult();
        }


        public async Task AddPermissionOverwriteAsync(ulong affectedId, PermissionOverwriteType type, DiscordPermission allow, DiscordPermission deny)
        {
            var overwrite = await Client.AddPermissionOverwriteAsync(Id, affectedId, type, allow, deny);
            List<DiscordPermissionOverwrite> overwrites = PermissionOverwrites.ToList();

            int i = overwrites.FindIndex(o => o.AffectedId == o.AffectedId);

            if (i == -1)
                overwrites.Add(overwrite);
            else
                overwrites[i] = overwrite;

            PermissionOverwrites = overwrites;
        }

        /// <summary>
        /// Adds/edits a permission overwrite to a channel
        /// </summary>
        public void AddPermissionOverwrite(ulong affectedId, PermissionOverwriteType type, DiscordPermission allow, DiscordPermission deny)
        {
            AddPermissionOverwriteAsync(affectedId, type, allow, deny).GetAwaiter().GetResult();
        }


        public async Task RemovePermissionOverwriteAsync(ulong affectedId)
        {
            await Client.RemovePermissionOverwriteAsync(Id, affectedId);

            try
            {
                List<DiscordPermissionOverwrite> overwrites = PermissionOverwrites.ToList();
                overwrites.RemoveAll(o => o.AffectedId == affectedId);
                PermissionOverwrites = overwrites;
            }
            catch { }
        }

        /// <summary>
        /// Removes a permission overwrite from a channel
        /// </summary>
        /// <param name="affectedId">ID of the role or member affected by the overwrite</param>
        public void RemovePermissionOverwrite(ulong affectedId)
        {
            RemovePermissionOverwriteAsync(affectedId).GetAwaiter().GetResult();
        }


        public string AsMessagable()
        {
            return $"<#{Id}>";
        }
    }
}