namespace Anarchy.Options;

using System.Net;
using System.Text.Json.Serialization;
using Discord.Gateway;

internal sealed class AccountOptions
{
    [JsonConstructor]
    public AccountOptions(ConnectionOptions connection, ulong channelId)
    {
        Connection = connection;
        ChannelId = channelId;
    }

    public ulong ChannelId { get; init; }
    public ConnectionOptions Connection { get; init; }
}

internal sealed class ConnectionOptions
{
    [JsonConstructor]
    public ConnectionOptions(string token, ProxyOptions? proxy)
    {
        Token = token;
        Proxy = proxy;
    }

    public DiscordGatewayIntent? Intents { get; init; }
    public ProxyOptions? Proxy { get; init; }
    public string Token { get; init; }
}

internal sealed class TestOptions
{
    [JsonConstructor]
    public TestOptions(AccountOptions bot, AccountOptions user)
    {
        Bot = bot;
        User = user;
    }

    public AccountOptions Bot { get; init; }
    public AccountOptions User { get; init; }
}

/// <summary>
/// The proxy that all clients available in <see cref="Globals.Options"/> will use.
/// </summary>
/// <remarks>
/// This is optional. If omitted the clients will not use a proxy.
/// </remarks>
/// <example>This example sets up the proxy as expected by <see
/// href="https://www.telerik.com/fiddler">Fiddler</see>.
/// <code>
/// "Proxy": { "Host": "127.0.0.1", "Port": 8888 }
/// </code>
/// </example>
internal sealed class ProxyOptions
{
    [JsonConstructor]
    public ProxyOptions(string host, int port)
    {
        Host = host;
        Port = port;
    }

    public string Host { get; init; }
    public int Port { get; init; }
    public IWebProxy Create()
    {
        return new WebProxy(Host, Port);
    }
}