using Newtonsoft.Json;
using System;
using System.Drawing;

namespace Discord
{
    public class DiscordUser : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("username")]
        public string Username { get; private set; }


        [JsonProperty("discriminator")]
        public uint Discriminator { get; private set; }


        [JsonProperty("avatar")]
        protected string _avatarHash;


        public DiscordUserAvatar Avatar
        {
            get
            {
                return new DiscordUserAvatar(Id, _avatarHash);
            }
        }


        [JsonProperty("public_flags")]
        public DiscordBadge PublicBadges { get; private set; }


        [JsonProperty("flags")]
        public DiscordBadge Badges { get; private set; }


        [JsonProperty("bot")]
#pragma warning disable 0414, 0649
        private readonly bool _bot;
#pragma warning restore 0414, 0649


        public DiscordUserType Type
        {
            get
            {
                if (Discriminator == 0)
                    return DiscordUserType.Webhook;
                else
                    return _bot ? DiscordUserType.Bot : DiscordUserType.User;
            }
        }



        public Hypesquad Hypesquad
        {
            get
            {
                return (Hypesquad)Enum.Parse(typeof(Hypesquad), 
                                        (PublicBadges & (DiscordBadge.HypeBravery | DiscordBadge.HypeBrilliance | DiscordBadge.HypeBalance))
                                         .ToString().Replace("Hype", ""));
            }
        }


        public DateTimeOffset CreatedAt
        {
            get { return DateTimeOffset.FromUnixTimeMilliseconds((long)((Id >> 22) + 1420070400000UL)); }
        }


        internal void Update(DiscordUser user)
        {
            Username = user.Username;
            Discriminator = user.Discriminator;
            _avatarHash = user._avatarHash;
            Badges = user.Badges;
            PublicBadges = user.PublicBadges;
        }


        /// <summary>
        /// Updates the user's info
        /// </summary>
        public void Update()
        {
            Update(Client.GetUser(Id));
        }


        /// <summary>
        /// Gets the user's profile
        /// </summary>
        public DiscordProfile GetProfile()
        {
            return Client.GetProfile(Id);
        }


        /// <summary>
        /// Sends a friend request to the user
        /// </summary>
        public void SendFriendRequest()
        {
            if (Id == Client.User.Id)
                throw new NotSupportedException("Cannot send a friend request to self.");

            Client.SendFriendRequest(Username, Discriminator);
        }


        /// <summary>
        /// Blocks the user
        /// </summary>
        public void Block()
        {
            if (Id == Client.User.Id)
                throw new NotSupportedException("Cannot block self.");

            Client.BlockUser(Id);
        }


        /// <summary>
        /// Removes any relationship (unfriending, unblocking etc.)
        /// </summary>
        public void RemoveRelationship()
        {
            if (Id == Client.User.Id)
                throw new NotSupportedException("Cannot remove relationship from self.");

            Client.RemoveRelationship(Id);
        }


        /// <summary>
        /// Gets the user's avatar
        /// </summary>
        /// <returns>The avatar (returns null if AvatarId is null)</returns>
        [Obsolete("GetAvatar is obsolete. Use Avatar.Download() instead", true)]
        public Image GetAvatar()
        {
            return null;
        }


        public string AsMessagable()
        {
            return $"<@{Id}>";
        }


        public override string ToString()
        {
            return $"{Username}#{"0000".Remove(4 - Discriminator.ToString().Length) + Discriminator.ToString()}";
        }


        public static implicit operator ulong(DiscordUser instance)
        {
            return instance.Id;
        }
    }
}