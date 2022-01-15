using Newtonsoft.Json;
using System;
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

        [JsonProperty("code")]
        public string Code { get; private set; }


        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("description")]
        public string Description { get; private set; }


        [JsonProperty("usage_count")]
        public int Usages { get; private set; }


        [JsonProperty("creator")]
        public DiscordUser Creator { get; private set; }


        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; private set; }


        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; private set; }


        [JsonProperty("source_guild_id")]
        private ulong _guildId;

        public MinimalGuild SourceGuild => new MinimalGuild(_guildId).SetClient(Client);


        [JsonProperty("serialized_source_guild")]
        private DiscordTemplateGuild _guild;

        public DiscordTemplateGuild Snapshot
        {
            get
            {
                _guild.SetGuildId(_guildId);

                return _guild;
            }
            private set => _guild = value;
        }


        [JsonProperty("is_dirty")]
        private readonly bool? _dirty;

        public bool HasUnsyncedChanges => _dirty.HasValue;


        public async Task UpdateAsync()
        {
            DiscordGuildTemplate template = await Client.GetGuildTemplateAsync(Code);
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
