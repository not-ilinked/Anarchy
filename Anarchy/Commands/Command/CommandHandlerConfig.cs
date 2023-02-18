using System;

namespace Discord.Commands;

public class CommandHandlerConfig
{
    public bool CaseInsensitiveCommands { get; set; } = false;

    /// <summary>
    /// Returns a string as prefix if custom prefix is found, else return null if not found.
    /// The custom prefixes could be stored in a database, and you can fetch them from this.
    /// </summary>
    public Func<ulong, string> GetGuildPrefix { get; set; } = (id) => null;
}