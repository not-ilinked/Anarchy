using Discord.Streaming;
using System.Threading.Tasks;

namespace Discord.Gateway
{
    internal static class StreamExtensions
    {
        public static Task<DiscordGoLiveSession> GoLiveAsync(this DiscordSocketClient client, ulong guildId, ulong channelId)
        {
            TaskCompletionSource<DiscordGoLiveSession> task = new TaskCompletionSource<DiscordGoLiveSession>();

            GoLiveCreate goLive = null;

            void createHandler(DiscordSocketClient c, GoLiveCreate stream)
            {
                client.OnStreamCreated -= createHandler;

                goLive = stream;
            }

            void serverHandler(DiscordSocketClient c, DiscordMediaServer server)
            {
                client.OnMediaServer -= serverHandler;

                server.GuildId = guildId;

                task.SetResult(new DiscordGoLiveSession(client, server, channelId, goLive));
            }

            client.OnStreamCreated += createHandler;
            client.OnMediaServer += serverHandler;

            client.Send(GatewayOpcode.GoLive, new StartStream() { GuildId = guildId, ChannelId = channelId, Type = "guild" });

            return task.Task;
        }


        public static void EndGoLive(this DiscordSocketClient client, string streamKey)
        {
            client.Send(GatewayOpcode.EndGoLive, new GoLiveStreamKey() { StreamKey = streamKey });
        }
    }
}
