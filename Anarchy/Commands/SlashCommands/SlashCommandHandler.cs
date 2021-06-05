using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Commands
{
    public class SlashCommandHandler
    {
        private class UnpackedSlashParameter
        {
            public PropertyInfo Property { get; set; }
            public SlashParameterAttribute Parameter { get; set; }
            public Dictionary<string, string> Choices { get; set; }
        }

        private class UnpackedSlashCommand
        {
            public Type HandlerType { get; set; }
            public SlashCommandAttribute Command { get; set; }
            public List<UnpackedSlashParameter> Parameters { get; set; }
        }

        private DiscordSocketClient _client;
        private ulong _appId;
        private List<UnpackedSlashCommand> _commands;

        public SlashCommandHandler(DiscordSocketClient client, ulong appId)
        {
            _client = client;
            _commands = new List<UnpackedSlashCommand>();
            _appId = appId;

            Assembly executable = Assembly.GetEntryAssembly();
            foreach (var type in executable.GetTypes())
            {
                if (typeof(SlashCommand).IsAssignableFrom(type))
                {
                    if (TryGetAttribute<SlashCommandAttribute>(type.GetCustomAttributes(), out var attr))
                    {
                        List<UnpackedSlashParameter> parameters = new List<UnpackedSlashParameter>();

                        foreach (var property in type.GetProperties())
                        {
                            if (TryGetAttribute<SlashParameterAttribute>(property.GetCustomAttributes(), out var paramAttr))
                            {
                                bool hasChoices = TryGetAttribute<SlashParameterChoicesAttribute>(property.GetCustomAttributes(), out var choicesAttr);

                                if (hasChoices && !(property.PropertyType == typeof(string) || IsInteger(property.PropertyType)))
                                    throw new InvalidOperationException($"[Command: {attr.Name}, parameter: {paramAttr.Name}] Only strings and integers can have choices");

                                parameters.Add(new UnpackedSlashParameter()
                                {
                                    Parameter = paramAttr,
                                    Property = property,
                                    Choices = hasChoices ? choicesAttr.Choices : null
                                });
                            }
                        }

                        _commands.Add(new UnpackedSlashCommand()
                        {
                            HandlerType = type,
                            Command = attr,
                            Parameters = parameters
                        });
                    }
                    else throw new MissingMemberException("All commands must have a SlashCommand attribute");
                }
            }

            Register();

            client.OnInteraction += Client_OnInteraction;
        }

        private void Client_OnInteraction(DiscordSocketClient client, DiscordInteractionEventArgs args)
        {
            if (args.Interaction.Type == DiscordInteractionType.ApplicationCommand && args.Interaction.ApplicationId == _appId)
            {
                foreach (var cmd in _commands)
                {
                    if (cmd.Command.Name == args.Interaction.Data.CommandName)
                    {
                        var handler = (SlashCommand)Activator.CreateInstance(cmd.HandlerType);
                        handler.Prepare(args.Interaction);

                        foreach (var suppliedArg in args.Interaction.Data.CommandArguments)
                        {
                            foreach (var param in cmd.Parameters)
                            {
                                if (param.Parameter.Name == suppliedArg.Name)
                                {
                                    object value = suppliedArg.Value;

                                    switch (suppliedArg.Type)
                                    {
                                        case CommandOptionType.Channel:
                                            value = args.Interaction.Data.Resolved.Channels[ulong.Parse(suppliedArg.Value)];
                                            break;
                                        case CommandOptionType.Role:
                                            value = args.Interaction.Data.Resolved.Roles[ulong.Parse(suppliedArg.Value)];
                                            break;
                                        case CommandOptionType.User:
                                            if (param.Property.PropertyType == typeof(DiscordUser)) value = args.Interaction.Data.Resolved.Users[ulong.Parse(suppliedArg.Value)];
                                            else value = args.Interaction.Data.Resolved.Members[ulong.Parse(suppliedArg.Value)];
                                            break;
                                        case CommandOptionType.Mentionable:
                                            // TODO
                                            break;
                                        default:
                                            value = Convert.ChangeType(value, param.Property.PropertyType);
                                            break;
                                    }

                                    param.Property.SetValue(handler, value);

                                    break;
                                }
                            }
                        }

                        if (cmd.Command.Delayed)
                        {
                            args.Interaction.Respond(InteractionCallbackType.DelayedMessage);
                            args.Interaction.ModifyResponse(handler.Handle());
                        }
                        else args.Interaction.Respond(InteractionCallbackType.RespondWithMessage, handler.Handle());

                        break;
                    }
                }
            }
        }

        private bool IsInteger(Type type) => type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong);

        private CommandOptionType ResolveType(Type type)
        {
            if (type == typeof(string)) return CommandOptionType.String;
            if (type == typeof(bool)) return CommandOptionType.Boolean;
            if (IsInteger(type)) return CommandOptionType.Integer;
            if (type == typeof(DiscordChannel)) return CommandOptionType.Channel;
            if (type == typeof(DiscordRole)) return CommandOptionType.Role;
            if (type == typeof(DiscordUser) || type == typeof(GuildMember)) return CommandOptionType.User;
            if (typeof(IMentionable).IsAssignableFrom(type)) return CommandOptionType.Mentionable;
            else throw new InvalidOperationException("Unexpected parameter type encountered");
        }

        private void Register()
        {
            List<ApplicationCommandProperties> commands = new List<ApplicationCommandProperties>();

            foreach (var cmd in _commands)
            {
                List<ApplicationCommandOption> options = new List<ApplicationCommandOption>();

                foreach (var parameter in cmd.Parameters)
                {
                    List<CommandOptionChoice> choices = null;

                    if (parameter.Choices != null && parameter.Choices.Count > 0)
                    {
                        choices = new List<CommandOptionChoice>();

                        foreach (var choice in parameter.Choices)
                        {
                            choices.Add(new CommandOptionChoice()
                            { 
                                Name = choice.Key,
                                Value = choice.Value
                            });
                        }
                    }

                    options.Add(new ApplicationCommandOption()
                    {
                        Name = parameter.Parameter.Name,
                        Description = parameter.Parameter.Description,
                        Required = parameter.Parameter.Required,
                        Choices = choices,
                        Type = ResolveType(parameter.Property.PropertyType)
                    });
                }

                commands.Add(new ApplicationCommandProperties()
                { 
                    Name = cmd.Command.Name,
                    Description = cmd.Command.Description,
                    Options = options
                });
            }

            _client.SetGlobalApplicationCommands(_appId, commands);
        }

        private static bool TryGetAttribute<TAttr>(IEnumerable<object> attributes, out TAttr attr) where TAttr : Attribute
        {
            foreach (var attribute in attributes)
            {
                if (attribute.GetType() == typeof(TAttr))
                {
                    attr = (TAttr)attribute;
                    return true;
                }
            }

            attr = null;
            return false;
        }
    }
}
