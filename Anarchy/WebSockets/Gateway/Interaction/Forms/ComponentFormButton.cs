using System.Threading.Tasks;

namespace Discord.Gateway
{
    public class ComponentFormButton : ComponentFormInput
    {
        public delegate void ClickHandler(object sender, FormInteractionEventArgs args);
        public event ClickHandler OnClick;

        public ComponentFormButton(MessageButtonStyle style, string text)
        {
            Style = style;
            Text = text;
        }

        public ComponentFormButton(MessageButtonStyle style, PartialEmoji emoji)
        {
            Style = style;
            Emoji = emoji;
        }

        public MessageButtonStyle Style { get; }
        public string Text { get; }
        public PartialEmoji Emoji { get; }
        public string RedirectUrl { get; set; }
        public bool Disabled { get; set; }

        internal override void Handle(DiscordSocketClient client, DiscordInteraction interaction)
        {
            if (OnClick != null)
                Task.Run(() => OnClick.Invoke(this, new FormInteractionEventArgs(client, interaction)));
        }
    }
}
