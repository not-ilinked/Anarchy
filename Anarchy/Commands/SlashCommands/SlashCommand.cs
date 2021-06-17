using Discord.Gateway;

namespace Discord.Commands
{
    public abstract class SlashCommand
    {
        public DiscordSocketClient Client { get; private set; }

        public DiscordUser Caller { get; private set; }
        public GuildMember CallerMember { get; private set; }

        public MinimalTextChannel Channel { get; private set; }
        public MinimalGuild Guild { get; private set; }

        internal void Prepare(DiscordInteraction interaction)
        {
            Client = (DiscordSocketClient)interaction.Client;

            Caller = interaction.User;
            CallerMember = interaction.Member;

            Channel = interaction.Channel;
            Guild = interaction.Guild;
        }

        public abstract InteractionResponseProperties Handle();
    }
}
