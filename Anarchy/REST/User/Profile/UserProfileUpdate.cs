using Newtonsoft.Json;
using System.Drawing;

namespace Discord
{
    /// <summary>
    /// Options for changing the account's profile
    /// </summary>
    public class UserProfileUpdate
    {
        private readonly Property<string> NameProperty = new Property<string>();
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


        internal Property<uint> DiscriminatorProperty = new Property<uint>();
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


        private readonly Property<string> EmailProperty = new Property<string>();
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


        private readonly Property<string> AvatarProperty = new Property<string>();
        [JsonProperty("avatar")]
        private string _avatar
        {
            get { return AvatarProperty; }
            set { AvatarProperty.Value = value; }
        }

        public Image Avatar
        {
            get { return DiscordImage.ToImage(_avatar); }
            set { _avatar = DiscordImage.FromImage(value); }
        }


        public bool ShouldSerialize_avatar()
        {
            return AvatarProperty.Set;
        }


        [JsonProperty("password")]
        public string Password { get; set; }


        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
    }
}