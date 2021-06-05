using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    public class ApplicationCommand : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        [JsonProperty("application_id")]
        public ulong ApplicationId { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("options")]
        public IReadOnlyList<ApplicationCommandOption> Options { get; private set; }


        private void Update(ApplicationCommand updated)
        {
            Name = updated.Name;
            Description = updated.Description;
            Options = updated.Options;
        }

        
        public async Task UpdateAsync() => Update(await Client.GetGlobalCommandAsync(ApplicationId, Id));
        public void Update() => UpdateAsync().GetAwaiter().GetResult();

        public async Task ModifyAsync(ApplicationCommandProperties properties) => Update(await Client.ModifyGlobalCommandAsync(ApplicationId, Id, properties));
        public void Modify(ApplicationCommandProperties properties) => ModifyAsync(properties).GetAwaiter().GetResult();

        public Task DeleteAsync() => Client.DeleteGlobalCommandAsync(ApplicationId, Id);
        public void Delete() => DeleteAsync().GetAwaiter().GetResult();
    }
}
