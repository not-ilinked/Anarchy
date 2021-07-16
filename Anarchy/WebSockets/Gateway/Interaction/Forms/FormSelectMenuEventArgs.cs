namespace Discord.Gateway
{
    public class FormSelectMenuEventArgs : FormInteractionEventArgs
    {
        public string[] Values { get; }

        public FormSelectMenuEventArgs(DiscordSocketClient client, DiscordInteraction interaction) : base(client, interaction)
        {
            Values = interaction.Data.SelectMenuValues;
        }
    }
}
