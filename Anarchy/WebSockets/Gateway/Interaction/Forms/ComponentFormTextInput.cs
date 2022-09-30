using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Gateway
{
    public class ComponentFormTextInput : ComponentFormInput
    {
        public delegate void SubmitHandler(object sender, FormInteractionEventArgs args);
        public event SubmitHandler OnSubmit;

        public ComponentFormTextInput(TextInputStyle style, string text)
        {
            Style = style;
            Text = text;
        }

        public TextInputStyle Style { get; }
        public string Text { get; }
        public string Placeholder { get; set; }
        public uint? MinLength { get; set; }
        public uint? MaxLength { get; set; }
        public bool Required { get; set; }

        internal override void Handle(DiscordSocketClient client, DiscordInteraction interaction)
        {
            if (OnSubmit != null)
                Task.Run(() => OnSubmit.Invoke(this, new FormInteractionEventArgs(client, interaction)));
        }
    }
}
