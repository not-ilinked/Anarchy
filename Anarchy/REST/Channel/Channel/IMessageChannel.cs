using System.Collections.Generic;

namespace Discord
{
    public interface IMessageChannel
    {
        void TriggerTyping();
        DiscordMessage SendMessage(string message, bool tts = false, DiscordEmbed embed = null);
        DiscordMessage SendFile(string fileName, byte[] fileData, string message = null, bool tts = false);
        DiscordMessage SendFile(string filePath, string message = null, bool tts = false);
        void DeleteMessages(List<ulong> messages);
        IReadOnlyList<DiscordMessage> GetMessages(MessageFilters filters = null);
        IReadOnlyList<DiscordMessage> GetPinnedMessages();
        void PinMessage(ulong messageId);
        void UnpinMessage(ulong messageId);
    }
}
