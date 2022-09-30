using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    public static class SlashCommandExtensions
    {
        public static async Task<IReadOnlyList<ApplicationCommand>> GetGlobalCommandsAsync(this DiscordClient client, ulong appId) =>
            (await client.HttpClient.GetAsync($"/applications/{appId}/commands")).Deserialize<List<ApplicationCommand>>().SetClientsInList(client);

        public static IReadOnlyList<ApplicationCommand> GetGlobalCommands(this DiscordClient client, ulong appId) => client.GetGlobalCommandsAsync(appId).GetAwaiter().GetResult();

        public static async Task<ApplicationCommand> GetGlobalCommandAsync(this DiscordClient client, ulong appId, ulong commandId) =>
            (await client.HttpClient.GetAsync($"/applications/{appId}/commands/{commandId}")).Deserialize<ApplicationCommand>().SetClient(client);

        public static ApplicationCommand GetApplicationCommand(this DiscordClient client, ulong appId, ulong commandId) => client.GetGlobalCommandAsync(appId, commandId).GetAwaiter().GetResult();


        public static async Task<ApplicationCommand> CreateGlobalCommandAsync(this DiscordClient client, ulong appId, ApplicationCommandProperties properties) =>
            (await client.HttpClient.PostAsync($"/applications/{appId}/commands", properties)).Deserialize<ApplicationCommand>().SetClient(client);

        public static ApplicationCommand CreateGlobalCommand(this DiscordClient client, ulong appId, ApplicationCommandProperties properties) => client.CreateGlobalCommandAsync(appId, properties).GetAwaiter().GetResult();


        public static async Task<IReadOnlyList<ApplicationCommand>> SetGlobalApplicationCommandsAsync(this DiscordClient client, ulong appId, List<ApplicationCommandProperties> commands) =>
            (await client.HttpClient.PutAsync($"/applications/{appId}/commands", commands)).Deserialize<List<ApplicationCommand>>().SetClientsInList(client);

        public static IReadOnlyList<ApplicationCommand> SetGlobalApplicationCommands(this DiscordClient client, ulong appId, List<ApplicationCommandProperties> commands) => client.SetGlobalApplicationCommandsAsync(appId, commands).GetAwaiter().GetResult();


        public static async Task<ApplicationCommand> ModifyGlobalCommandAsync(this DiscordClient client, ulong appId, ulong commandId, ApplicationCommandProperties properties) =>
            (await client.HttpClient.PatchAsync($"/applications/{appId}/commands/{commandId}", properties)).Deserialize<ApplicationCommand>().SetClient(client);

        public static ApplicationCommand ModifyGlobalCommand(this DiscordClient client, ulong appId, ulong commandId, ApplicationCommandProperties properties) => client.ModifyGlobalCommandAsync(appId, commandId, properties).GetAwaiter().GetResult();


        public static Task DeleteGlobalCommandAsync(this DiscordClient client, ulong appId, ulong commandId) => client.HttpClient.DeleteAsync($"/applications/{appId}/commands/{commandId}");
        public static void DeleteGlobalCommand(this DiscordClient client, ulong appId, ulong commandId) => client.DeleteGlobalCommandAsync(appId, commandId).GetAwaiter().GetResult();


        public static async Task<IReadOnlyList<ApplicationCommand>> GetGuildCommandsAsync(this DiscordClient client, ulong appId, ulong guildID) =>
            (await client.HttpClient.GetAsync($"/applications/{appId}/guilds/{guildID}/commands")).Deserialize<List<ApplicationCommand>>().SetClientsInList(client);

        public static IReadOnlyList<ApplicationCommand> GetGuildCommands(this DiscordClient client, ulong appId, ulong guildID) => client.GetGuildCommandsAsync(appId, guildID).GetAwaiter().GetResult();


        public static async Task<ApplicationCommand> GetGuildCommandAsync(this DiscordClient client, ulong appId, ulong guildID, ulong commandId) =>
            (await client.HttpClient.GetAsync($"/applications/{appId}/guilds/{guildID}/commands/{commandId}")).Deserialize<ApplicationCommand>().SetClient(client);

        public static ApplicationCommand GetGuildApplicationCommand(this DiscordClient client, ulong appId, ulong guildID, ulong commandId) => client.GetGuildCommandAsync(appId, guildID, commandId).GetAwaiter().GetResult();


        public static async Task<ApplicationCommand> CreateGuildCommandAsync(this DiscordClient client, ulong appId, ulong guildID, ApplicationCommandProperties properties) =>
            (await client.HttpClient.PostAsync($"/applications/{appId}/guilds/{guildID}/commands", properties)).Deserialize<ApplicationCommand>().SetClient(client);

        public static ApplicationCommand CreateGuildCommand(this DiscordClient client, ulong appId, ulong guildID, ApplicationCommandProperties properties) => client.CreateGuildCommandAsync(appId, guildID, properties).GetAwaiter().GetResult();


        public static async Task<IReadOnlyList<ApplicationCommand>> SetGuildApplicationCommandsAsync(this DiscordClient client, ulong appId, ulong guildID, List<ApplicationCommandProperties> commands) =>
            (await client.HttpClient.PutAsync($"/applications/{appId}/guilds/{guildID}/commands", commands)).Deserialize<List<ApplicationCommand>>().SetClientsInList(client);

        public static IReadOnlyList<ApplicationCommand> SetGuildApplicationCommands(this DiscordClient client, ulong appId, ulong guildID, List<ApplicationCommandProperties> commands) => client.SetGuildApplicationCommandsAsync(appId, guildID, commands).GetAwaiter().GetResult();


        public static async Task<ApplicationCommand> ModifyGuildCommandAsync(this DiscordClient client, ulong appId, ulong guildID, ulong commandId, ApplicationCommandProperties properties) =>
            (await client.HttpClient.PatchAsync($"/applications/{appId}/guilds/{guildID}/commands/{commandId}", properties)).Deserialize<ApplicationCommand>().SetClient(client);

        public static ApplicationCommand ModifyGuildCommand(this DiscordClient client, ulong appId, ulong guildID, ulong commandId, ApplicationCommandProperties properties) => client.ModifyGuildCommandAsync(appId, guildID, commandId, properties).GetAwaiter().GetResult();


        public static Task DeleteGuildCommandAsync(this DiscordClient client, ulong appId, ulong guildID, ulong commandId) => client.HttpClient.DeleteAsync($"/applications/{appId}/guilds/{guildID}/commands/{commandId}");
        public static void DeleteGuildCommand(this DiscordClient client, ulong appId, ulong guildID, ulong commandId) => client.DeleteGuildCommandAsync(appId, guildID, commandId).GetAwaiter().GetResult();


        public static List<ApplicationCommand> SearchApplicationCommands(this DiscordClient client, ulong appId, ulong channelId)
        {
            var sRes = client.HttpClient.GetAsync($"/channels/{channelId}/application-commands/search?type=1&application_id={appId}").GetAwaiter().GetResult();
            if (sRes != null)
            {
                var AppCommand = sRes.Body["application_commands"];
                if (AppCommand != null)
                {
                    return AppCommand.ToObject<List<ApplicationCommand>>();
                }
            }
            return null;
        }
    }
}
