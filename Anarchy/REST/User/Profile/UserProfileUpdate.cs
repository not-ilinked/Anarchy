﻿using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for changing the account's profile
    /// </summary>
    public class UserProfileUpdate
    {
        private readonly DiscordParameter<string> NameProperty = new DiscordParameter<string>();
        [JsonProperty("username")]
        public string Username
        {
            get { return NameProperty; }
            set { NameProperty.Value = value; }
        }


        public bool ShouldSerializeUsername()
        {
            return NameProperty.Set;
        }


        internal DiscordParameter<uint> DiscriminatorProperty = new DiscordParameter<uint>();
        [JsonProperty("discriminator")]
        public uint Discriminator
        {
            get { return DiscriminatorProperty; }
            set { DiscriminatorProperty.Value = value; }
        }


        public bool ShouldSerializeDiscriminator()
        {
            return DiscriminatorProperty.Set;
        }


        private readonly DiscordParameter<string> EmailProperty = new DiscordParameter<string>();
        [JsonProperty("email")]
        public string Email
        {
            get { return EmailProperty; }
            set { EmailProperty.Value = value; }
        }


        public bool ShouldSerializeEmail()
        {
            return EmailProperty.Set;
        }


        private readonly DiscordParameter<DiscordImage> AvatarProperty = new DiscordParameter<DiscordImage>();
        [JsonProperty("avatar")]
        public DiscordImage Avatar
        {
            get { return AvatarProperty; }
            set { AvatarProperty.Value = value; }
        }


        public bool ShouldSerializeAvatar()
        {
            return AvatarProperty.Set;
        }


        private readonly DiscordParameter<string> _bioParam = new DiscordParameter<string>();
        [JsonProperty("bio")]
        public string Biography
        {
            get { return _bioParam; }
            set { _bioParam.Value = value; }
        }

        public bool ShouldSerializeBiography() => _bioParam.Set;


        private readonly DiscordParameter<DiscordImage> _bannerParam = new DiscordParameter<DiscordImage>();
        [JsonProperty("banner")]
        public DiscordImage Banner
        {
            get { return _bannerParam; }
            set { _bannerParam.Value = value; }
        }

        public bool ShouldSerializeBanner() => _bannerParam.Set;



        [JsonProperty("password")]
        public string Password { get; set; }


        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
    }
}