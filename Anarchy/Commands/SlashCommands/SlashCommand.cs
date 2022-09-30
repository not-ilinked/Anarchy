using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Discord.Gateway;
using Newtonsoft.Json;

namespace Discord.Commands
{
    public abstract class SlashCommand
    {
        public DiscordSocketClient Client { get; private set; }
        public DiscordInteraction Interaction { get; private set; }

        public DiscordUser Caller { get; private set; }
        public GuildMember CallerMember { get; private set; }

        public MinimalTextChannel Channel { get; private set; }
        public MinimalGuild Guild { get; private set; }

        internal void Prepare(DiscordInteraction interaction)
        {
            Client = (DiscordSocketClient)interaction.Client;
            Interaction = interaction;

            Caller = interaction.User;
            CallerMember = interaction.Member;

            Channel = interaction.Channel;
            Guild = interaction.Guild;
        }

        public abstract InteractionResponseProperties Handle();
        public virtual InteractionResponseProperties HandleAutoComplete() { return null; }
        public virtual InteractionResponseProperties HandleModalSubmit() { return null; }

        protected InteractionResponseProperties ShowModal(string modalTitle)
        {
            var type = this.GetType();
            TryGetAttribute<SlashCommandAttribute>(type.GetCustomAttributes(), out var attr);
            TryGetAttribute<SlashCommandCategoryAttribute>(type.GetCustomAttributes(), out var catAttr);

            string category = catAttr?.Category;
            string subcommandgroup = catAttr?.SubcommandGroup;
            string sName = "";
            if (category != null)
            {
                sName += category + ".";
                if (subcommandgroup != null) sName += subcommandgroup + ".";
            }
            sName += attr.Name + ".";

            var prop = new InteractionResponseProperties
            {
                Title = modalTitle,
                CustomID = sName.Trim('.')
            };

            List<MessageComponent> components = new List<MessageComponent>();

            foreach (var property in type.GetProperties())
            {
                if (TryGetAttribute<ModalParameterAttribute>(property.GetCustomAttributes(), out var paramAttr))
                {
                    List<MessageInputComponent> input = new List<MessageInputComponent>();
                    var listInputComp = new TextInputComponent()
                    {
                        Id = paramAttr.Id,
                        Text = paramAttr.Text,
                        Style = paramAttr.Style,
                        MinLength = paramAttr.MinLength,
                        MaxLength = paramAttr.MaxLength,
                        Placeholder = paramAttr.PlaceHolder,
                        Required = paramAttr.Required,
                    };
                    input.Add(listInputComp);

                    components.Add(new RowComponent(input));
                }
            }

            prop.Components = components;

            Interaction.Respond(InteractionCallbackType.Modal, prop);

            return null;
        }

        protected InteractionResponseProperties ShowAutocompleteChoices(string sValue, List<string> lChoices)
        {
            var listChoices = new List<CommandOptionChoice>();

            var i = 0;
            //build list choices
            foreach (var item in lChoices)
            {
                if (item.ToLower().IndexOf(sValue.ToLower()) == 0 || sValue == "")
                {
                    listChoices.Add(new CommandOptionChoice() { Name = item, Value = item });
                    i++;
                }
                //Discord only for up to 25 choices
                if (i == 25)
                {
                    break;
                }
            }

            var prop = new InteractionResponseProperties
            {
                Choices = listChoices
            };
            Interaction.Respond(InteractionCallbackType.ApplicationCommandAutocompleteResult, prop);

            return null;
        }

        protected SlashCommandArgument GetFocusedOption()
        {
            var opts = Interaction.Data.CommandArguments[0].Options;

            while (opts[0].Options != null)
            {
                if (opts[0].Focused)
                {
                    break;
                }
                opts = opts[0].Options;
            }

            foreach (var opt in opts)
            {
                if (opt.Focused)
                {
                    return opt;
                }
            }

            return null;
        }

        private static bool TryGetAttribute<TAttr>(IEnumerable<object> attributes, out TAttr attr) where TAttr : Attribute
        {
            foreach (var attribute in attributes)
            {
                if (attribute.GetType() == typeof(TAttr))
                {
                    attr = (TAttr) attribute;
                    return true;
                }
            }

            attr = null;
            return false;
        }
    }
}
