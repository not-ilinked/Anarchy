﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


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

        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("icon")]
        private string _iconHash;

        public DiscordCDNImage Icon
        {
            get
            {
                if (_iconHash == null)
                    return null;
                else
                    return new DiscordCDNImage(CDNEndpoints.AppIcon, Id, _iconHash);
            }
        }

        [JsonPropertyName("description")]
        public string Description { get; private set; }

        [JsonPropertyName("summary")]
        public string Summary { get; private set; }

        [JsonPropertyName("verify_key")]
        public string VerifyKey { get; private set; }

        [JsonPropertyName("bot_public")]
        public bool PublicBot { get; private set; }

        [JsonPropertyName("bot_require_code_grant")]
        public bool RequiresCodeGrant { get; private set; }

        [JsonPropertyName("owner")]
        public DiscordUser Owner { get; private set; }

        [JsonPropertyName("bot")]
        public ApplicationBot Bot { get; private set; }

        [JsonPropertyName("redirect_uris")]
        public IReadOnlyList<string> RedirectUris { get; private set; }

        private void Update(OAuth2Application app)
        {
            Name = app.Name;
            _iconHash = app._iconHash;
            Description = app.Description;
            Summary = app.Summary;
            VerifyKey = app.VerifyKey;
            PublicBot = app.PublicBot;
            RequiresCodeGrant = app.RequiresCodeGrant;
            Owner = app.Owner;
            Bot = app.Bot;
            RedirectUris = app.RedirectUris;
        }

        public async Task UpdateAsync()
        {
            Update(await Client.GetApplicationAsync(Id));
        }

        public void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }

        public async Task ModifyAsync(DiscordApplicationProperties properties)
        {
            Update(await Client.ModifyApplicationAsync(Id, properties));
        }

        public void Modify(DiscordApplicationProperties properties)
        {
            ModifyAsync(properties).GetAwaiter().GetResult();
        }

        public ApplicationBot AddBot()
        {
            return Bot = Client.AddBotToApplication(Id);
        }

        public async Task DeleteAsync()
        {
            await Client.DeleteApplicationAsync(Id);
        }

        public void Delete()
        {
            DeleteAsync().GetAwaiter().GetResult();
        }

        public static implicit operator ulong(OAuth2Application instance)
        {
            return instance.Id;
        }
    }
}
