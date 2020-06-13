using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace Discord
{
    public class OAuth2Application : Controllable
    {
        public OAuth2Application()
        {
            OnClientUpdated += (sender, e) =>
            {
                if (Bot != null)
                    Bot.SetClient(Client);
            };
        }


        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("icon")]
        private string _icon;

        public DiscordAppIconCDNImage Icon
        {
            get { return new DiscordAppIconCDNImage(Id, _icon); }
        }


        [JsonProperty("description")]
        public string Description { get; private set; }


        [JsonProperty("summary")]
        public string Summary { get; private set; }


        [JsonProperty("verify_key")]
        public string VerifyKey { get; private set; }


        [JsonProperty("bot_public")]
        public bool PublicBot { get; private set; }


        [JsonProperty("bot_require_code_grant")]
        public bool RequiresCodeGrant { get; private set; }


        [JsonProperty("owner")]
        public DiscordUser Owner { get; private set; }


        [JsonProperty("bot")]
        public ApplicationBot Bot { get; private set; }


        [JsonProperty("redirect_uris")]
        public IReadOnlyList<string> RedirectUris { get; private set; }


        private void _updateProperties(OAuth2Application app)
        {
            Name = app.Name;
            _icon = app.Icon.Hash;
            Description = app.Description;
            Summary = app.Summary;
            VerifyKey = app.VerifyKey;
            PublicBot = app.PublicBot;
            RequiresCodeGrant = app.RequiresCodeGrant;
            Owner = app.Owner;
            Bot = app.Bot;
            RedirectUris = app.RedirectUris;
        }


        public void Update()
        {
            _updateProperties(Client.GetApplication(Id));
        }


        public void Modify(DiscordApplicationProperties properties)
        {
            _updateProperties(Client.ModifyApplication(Id, properties));
        }


        public ApplicationBot AddBot()
        {
            return Bot = Client.AddBotToApplication(Id);
        }


        public void Delete()
        {
            Client.DeleteApplication(Id);
        }


        /// <summary>
        /// Gets the application's icon
        /// </summary>
        /// <returns>The icon (returns null if IconId is null)</returns>
        [Obsolete("GetIcon is obsolete. Use Icon.Download() instead")]
        public Image GetAvatar()
        {
            return null;
        }


        public static implicit operator ulong(OAuth2Application instance)
        {
            return instance.Id;
        }
    }
}
