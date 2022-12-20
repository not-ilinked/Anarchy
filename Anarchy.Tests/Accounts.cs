namespace Anarchy;

using System.Text.Json;
using Anarchy.Options;

internal static class Accounts
{
    static Accounts()
    {
        var options = GetOptions();

        Bot = new Account(options.Bot);
        User = new Account(options.User);
    }

    public static Account Bot { get; set; }
    public static Account User { get; set; }

    private static TestOptions GetOptions()
    {
        const string exampleFilename = @".\appsettings.Example.json";
        const string optionsFilename = @".\appsettings.Tests.json";

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), optionsFilename);
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"The file {optionsFilename} was not found. Create one using {exampleFilename} as a template.", filePath);

        var options = JsonSerializer.Deserialize<TestOptions>(File.ReadAllText(filePath));
        if (options is null)
            throw new FormatException($"The file {optionsFilename} was incomplete or formatted incorrectly.");

        return options;
    }
}