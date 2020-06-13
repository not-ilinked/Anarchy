using Discord.Gateway;

namespace Discord.Commands
{
    public interface ICommand
    {
        void Execute(DiscordSocketClient client, DiscordMessage message);
    }
}
