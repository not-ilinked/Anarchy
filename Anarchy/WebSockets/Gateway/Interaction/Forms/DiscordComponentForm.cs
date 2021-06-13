using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Gateway
{
    public class DiscordComponentForm
    {
        private DiscordSocketClient _client;

        internal string Id { get; }
        public List<List<ComponentFormButton>> Rows { get; }

        public DiscordComponentForm(DiscordSocketClient client)
        {
            _client = client;

            _client.OnInteraction += HandleInteraction;

            Id = RandomString(16);
            Rows = new List<List<ComponentFormButton>>();
        }

        public DiscordComponentForm(DiscordSocketClient client, List<List<ComponentFormButton>> rows) : this(client)
        {
            Rows = rows;
        }

        private void HandleInteraction(DiscordSocketClient client, DiscordInteractionEventArgs args)
        {
            if (args.Interaction.Type == DiscordInteractionType.MessageComponent)
            {
                string[] parts = args.Interaction.Data.ComponentId.Split('-');

                if (parts[0] == Id)
                {
                    foreach (var row in Rows)
                    {
                        foreach (var button in row)
                        {
                            if (button.Id == parts[1])
                                button.TriggerClick(_client, args.Interaction);
                        }
                    }
                }
            }
        }

        private static Random random = new Random();
        internal static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static implicit operator List<MessageComponent>(DiscordComponentForm instance)
        {
            List<MessageComponent> components = new List<MessageComponent>();

            foreach (var row in instance.Rows)
            {
                List<MessageComponent> buttons = new List<MessageComponent>();

                foreach (var button in row)
                {
                    buttons.Add(new ButtonComponent()
                    {
                        Id = $"{instance.Id}-{button.Id}",
                        Text = button.Text,
                        Style = button.Style,
                        Disabled = button.Disabled,
                        Emoji = button.Emoji,
                        RedirectUrl = button.RedirectUrl
                    });
                }

                components.Add(new RowComponent(buttons));
            }

            return components;
        }
    }
}
