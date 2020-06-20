using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

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


        /// <summary>
        /// Updates the channel
        /// </summary>
        public new void Update()
        {
            Update(Client.GetChannel(Id).ToGuildChannel());
        }


        /// <summary>
        /// Modifies the channel
        /// </summary>
        /// <param name="properties">Options for modifying the channel</param>
        public void Modify(GuildChannelProperties properties)
        {
            Update(Client.ModifyChannel(Id, properties));
        }


        /// <summary>
        /// Adds/edits a permission overwrite to a channel
        /// </summary>
        /// <param name="overwrite">The permission overwrite to add/edit</param>
        public void AddPermissionOverwrite(DiscordPermissionOverwrite overwrite)
        {
            Client.AddPermissionOverwrite(Id, overwrite);
            List<DiscordPermissionOverwrite> overwrites = PermissionOverwrites.ToList();
            if (overwrites.Where(pe => pe.Id == overwrite.Id).Count() > 0)
                overwrites[overwrites.IndexOf(overwrites.First(pe => pe.Id == overwrite.Id))] = overwrite;
            else
                overwrites.Add(overwrite);
            PermissionOverwrites = overwrites;
        }


        /// <summary>
        /// Removes a permission overwrite from a channel
        /// </summary>
        /// <param name="id">ID of the role or member affected by the overwrite</param>
        public void RemovePermissionOverwrite(ulong id)
        {
            Client.RemovePermissionOverwrite(Id, id);

            try
            {
                List<DiscordPermissionOverwrite> overwrites = PermissionOverwrites.ToList();
                overwrites.Remove(PermissionOverwrites.First(pe => pe.Id == id));
                PermissionOverwrites = overwrites;
            }
            catch { }
        }


        public string AsMessagable()
        {
            return $"<#{Id}>";
        }
    }
}