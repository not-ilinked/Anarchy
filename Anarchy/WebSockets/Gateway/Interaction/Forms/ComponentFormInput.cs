namespace Discord.Gateway
{
    public abstract class ComponentFormInput
    {
        internal string Id { get; } = DiscordComponentForm.RandomString(8);
        internal abstract void Handle(DiscordSocketClient client, DiscordInteraction interaction);
    }
}
