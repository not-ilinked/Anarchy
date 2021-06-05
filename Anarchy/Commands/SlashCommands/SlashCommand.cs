using Discord.Gateway;

namespace Discord.Commands
{
    public abstract class SlashCommand
    {
        public DiscordSocketClient Client { get; private set; }

        public DiscordUser User { get; private set; }
        public GuildMember Member { get; private set; }

        public MinimalTextChannel Channel { get; private set; }
        public MinimalGuild Guild { get; private set; }

        internal void Prepare(DiscordInteraction interaction)
        {
            Client = (DiscordSocketClient)interaction.Client;

            User = interaction.User;
            Member = interaction.Member;

            Channel = interaction.Channel;
            Guild = interaction.Guild;
        }

        public abstract InteractionCallbackProperties Handle();
    }
}
