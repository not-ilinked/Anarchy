using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Discord
{
    public class DiscordGuildTemplate : Controllable
    {
        public DiscordGuildTemplate()
        {
            OnClientUpdated += (sender, e) =>
            {
                Creator.SetClient(Client);
                SourceGuild.SetClient(Client);
                Snapshot.SetClient(Client);
            };
        }

        [JsonPropertyName("code")]
        public string Code { get; private set; }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("description")]
        public string Description { get; private set; }

        [JsonPropertyName("usage_count")]
        public int Usages { get; private set; }

        [JsonPropertyName("creator")]
        public DiscordUser Creator { get; private set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; private set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; private set; }

        [JsonPropertyName("source_guild_id")]
        private ulong _guildId;

        public MinimalGuild SourceGuild
        {
            get { return new MinimalGuild(_guildId).SetClient(Client); }
        }

        [JsonPropertyName("serialized_source_guild")]
        private DiscordTemplateGuild _guild;

        public DiscordTemplateGuild Snapshot
        {
            get
            {
                _guild.SetGuildId(_guildId);

                return _guild;
            }
            private set
            {
                _guild = value;
            }
        }

        [JsonPropertyName("is_dirty")]
        private readonly bool? _dirty;

        public bool HasUnsyncedChanges
        {
            get { return _dirty.HasValue; }
        }

        public async Task UpdateAsync()
        {
            var template = await Client.GetGuildTemplateAsync(Code);
            Name = template.Name;
            Usages = template.Usages;
            Creator = template.Creator;
            _guildId = template._guildId;
        }

        public void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }
    }
}
