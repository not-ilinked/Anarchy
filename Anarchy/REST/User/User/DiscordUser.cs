using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Threading.Tasks;

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
        private readonly bool _bot;


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


        public async Task UpdateAsync()
        {
            Update(await Client.GetUserAsync(Id));
        }

        /// <summary>
        /// Updates the user's info
        /// </summary>
        public void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }


        public async Task<DiscordProfile> GetProfileAsync()
        {
            return await Client.GetProfileAsync(Id);
        }

        /// <summary>
        /// Gets the user's profile
        /// </summary>
        public DiscordProfile GetProfile()
        {
            return GetProfileAsync().Result;
        }


        public async Task SendFriendRequestAsync()
        {
            if (Id == Client.User.Id)
                throw new NotSupportedException("Cannot send a friend request to self.");

            await Client.SendFriendRequestAsync(Username, Discriminator);
        }

        /// <summary>
        /// Sends a friend request to the user
        /// </summary>
        public void SendFriendRequest()
        {
            SendFriendRequestAsync().GetAwaiter().GetResult();
        }


        public async Task BlockAsync()
        {
            if (Id == Client.User.Id)
                throw new NotSupportedException("Cannot block self.");

            await Client.BlockUserAsync(Id);
        }

        /// <summary>
        /// Blocks the user
        /// </summary>
        public void Block()
        {
            BlockAsync().GetAwaiter().GetResult();
        }


        public async Task RemoveRelationshipAsync()
        {
            if (Id == Client.User.Id)
                throw new NotSupportedException("Cannot remove relationship from self.");

            await Client.RemoveRelationshipAsync(Id);
        }

        /// <summary>
        /// Removes any relationship (unfriending, unblocking etc.)
        /// </summary>
        public void RemoveRelationship()
        {
            RemoveRelationshipAsync().GetAwaiter().GetResult();
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