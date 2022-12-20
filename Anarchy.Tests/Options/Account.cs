namespace Anarchy.Options;

internal sealed class Account
{
    public Account(AccountOptions options)
    {
        Options = options;
        Clients = new Clients(options.Connection);
    }

    public Clients Clients { get; set; }
    public AccountOptions Options { get; set; }
}