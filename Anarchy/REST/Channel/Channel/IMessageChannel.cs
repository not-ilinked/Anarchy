using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    public interface IMessageChannel
    {
        Task TriggerTypingAsync();
        void TriggerTyping();

        Task<DiscordMessage> SendMessageAsync(MessageProperties properties);
        DiscordMessage SendMessage(MessageProperties properties);

        Task<DiscordMessage> SendMessageAsync(string message, bool tts = false, DiscordEmbed embed = null);
        DiscordMessage SendMessage(string message, bool tts = false, DiscordEmbed embed = null);

        Task<DiscordMessage> SendMessageAsync(EmbedMaker embed);
        DiscordMessage SendMessage(EmbedMaker embed);

        Task<DiscordMessage> SendFileAsync(string fileName, byte[] fileData, string message = null, bool tts = false);
        DiscordMessage SendFile(string fileName, byte[] fileData, string message = null, bool tts = false);

        Task<DiscordMessage> SendFileAsync(string filePath, string message = null, bool tts = false);
        DiscordMessage SendFile(string filePath, string message = null, bool tts = false);

        Task DeleteMessagesAsync(List<ulong> messages);
        void DeleteMessages(List<ulong> messages);

        Task<IReadOnlyList<DiscordMessage>> GetMessagesAsync(MessageFilters filters = null);
        IReadOnlyList<DiscordMessage> GetMessages(MessageFilters filters = null);

        Task<IReadOnlyList<DiscordMessage>> GetPinnedMessagesAsync();
        IReadOnlyList<DiscordMessage> GetPinnedMessages();

        Task PinMessageAsync(ulong messageId);
        void PinMessage(ulong messageId);

        Task UnpinMessageAsync(ulong messageId);
        void UnpinMessage(ulong messageId);
    }
}
