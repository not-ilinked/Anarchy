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
        public void AddPermissionOverwrite(ulong affectedId, PermissionOverwriteType type, DiscordPermission allow, DiscordPermission deny)
        {
            var overwrite = new DiscordPermissionOverwrite() { AffectedId = affectedId, Type = type, Allow = allow, Deny = deny };

            Client.AddPermissionOverwrite(Id, overwrite);
            List<DiscordPermissionOverwrite> overwrites = PermissionOverwrites.ToList();

            int i = overwrites.FindIndex(o => o.AffectedId == o.AffectedId);

            if (i == -1)
                overwrites.Add(overwrite);
            else
                overwrites[i] = overwrite;

            PermissionOverwrites = overwrites;
        }


        /// <summary>
        /// Removes a permission overwrite from a channel
        /// </summary>
        /// <param name="affectedId">ID of the role or member affected by the overwrite</param>
        public void RemovePermissionOverwrite(ulong affectedId)
        {
            Client.RemovePermissionOverwrite(Id, affectedId);

            try
            {
                List<DiscordPermissionOverwrite> overwrites = PermissionOverwrites.ToList();
                overwrites.RemoveAll(o => o.AffectedId == affectedId);
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