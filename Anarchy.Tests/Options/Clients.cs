namespace Anarchy.Options;

using Discord;
using Discord.Gateway;

internal sealed class Clients
{
    private readonly ConnectionOptions _options;
    private DiscordSocketClient? _gatewayClient;
    private DiscordClient? _restClient;

    public Clients(ConnectionOptions options)
    {
        _options = options;
    }

    public DiscordSocketClient GatewayClient
    {
        get { return _gatewayClient ??= CreateGatewayClient(); }
    }

    public DiscordClient RestClient
    {
        get { return _restClient ??= CreateRestClient(); }
    }

    private DiscordSocketClient CreateGatewayClient()
    {
        var autoResetEvent = new AutoResetEvent(false);

        var client = new DiscordSocketClient(new DiscordSocketConfig()
        {
            Proxy = _options.Proxy?.Create(),
            Intents = _options.Intents
        });

        void OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine($"Logged into {args.User}");
            autoResetEvent.Set();
        }

        client.OnLoggedIn += OnLoggedIn;
        client.Login(_options.Token);

        autoResetEvent.WaitOne();

        return client;
    }

    private DiscordClient CreateRestClient()
    {
        return new DiscordClient(_options.Token, new DiscordSocketConfig()
        {
            Proxy = _options.Proxy?.Create()
        });
    }
}