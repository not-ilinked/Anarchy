using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord.Gateway
{
    public class DiscordComponentForm
    {
        private DiscordSocketClient _client;

        internal string Id { get; }
        public List<List<ComponentFormInput>> Rows { get; }

        public DiscordComponentForm(DiscordSocketClient client)
        {
            _client = client;
            _client.OnInteraction += HandleInteraction;

            Id = RandomString(16);
            Rows = new List<List<ComponentFormInput>>();
        }

        public DiscordComponentForm(DiscordSocketClient client, List<List<ComponentFormInput>> rows) : this(client)
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
                                button.Handle(_client, args.Interaction);
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
                List<MessageInputComponent> inputs = new List<MessageInputComponent>();

                foreach (var input in row)
                {
                    if (input.GetType() == typeof(ComponentFormSelectMenu))
                    {
                        var asSelect = (ComponentFormSelectMenu)input;
                        inputs.Add(new SelectMenuComponent()
                        {
                            Id = $"{instance.Id}-{asSelect.Id}",
                            Disabled = asSelect.Disabled,
                            Placeholder = asSelect.Placeholder,
                            MaxSelected = asSelect.MaxSelected,
                            MinimumSelected = asSelect.MinimumSelected,
                            Options = asSelect.Options
                        });
                    }
                    else if (input.GetType() == typeof(ComponentFormButton))
                    {
                        var asButton = (ComponentFormButton)input;

                        inputs.Add(new ButtonComponent()
                        {
                            Id = $"{instance.Id}-{asButton.Id}",
                            Text = asButton.Text,
                            Style = asButton.Style,
                            Disabled = asButton.Disabled,
                            Emoji = asButton.Emoji,
                            RedirectUrl = asButton.RedirectUrl
                        });
                    } else if (input.GetType() == typeof(ComponentFormTextInput))
                    {
                        var asTextInput = (ComponentFormTextInput) input;

                        inputs.Add(new TextInputComponent()
                        {
                            Id = $"{instance.Id}-{asTextInput.Id}",
                            Text = asTextInput.Text,
                            Style = asTextInput.Style,
                            Placeholder = asTextInput.Placeholder,
                            MaxLength = asTextInput.MaxLength,
                            MinLength = asTextInput.MinLength,
                            Required = asTextInput.Required,
                        });
                    }
                }

                components.Add(new RowComponent(inputs));
            }

            return components;
        }
    }
}
