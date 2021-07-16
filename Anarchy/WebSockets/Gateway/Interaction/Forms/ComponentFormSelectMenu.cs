using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Gateway
{
    public class ComponentFormSelectMenu : ComponentFormInput
    {
        public delegate void SelectHandler(object sender, FormSelectMenuEventArgs args);
        public event SelectHandler OnSelect;

        public List<SelectMenuOption> Options { get; }
        public string Placeholder { get; set; }
        public uint? MinimumSelected { get; set; }
        public uint? MaxSelected { get; set; }
        public bool Disabled { get; set; }

        public ComponentFormSelectMenu(List<SelectMenuOption> options)
        {
            Options = options;
        }

        internal override void Handle(DiscordSocketClient client, DiscordInteraction interaction)
        {
            if (OnSelect != null)
                Task.Run(() => OnSelect.Invoke(this, new FormSelectMenuEventArgs(client, interaction)));
        }
    }
}
