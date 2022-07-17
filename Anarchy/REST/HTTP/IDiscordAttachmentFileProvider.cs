using System.Collections.Generic;

namespace Discord
{
    internal interface IDiscordAttachmentFileProvider
    {
        IEnumerable<(string FileName, DiscordAttachmentFile File, int Id)> GetAttachmentFiles();
    }
}
