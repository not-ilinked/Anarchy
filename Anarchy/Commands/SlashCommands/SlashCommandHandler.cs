using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Discord.Gateway;

namespace Discord.Commands
{
    internal class SlashCommandHandler
    {
        private class LocalCommandInfo
        {
            public Type HandlerType { get; set; }
            public Dictionary<string, PropertyInfo> Parameters { get; set; }
            public Dictionary<string, PropertyInfo> ModalParameters { get; set; }
            public bool Delayed { get; set; }
        }

        private DiscordSocketClient _client;
        public ulong ApplicationId { get; private set; }
        private List<ApplicationCommandProperties> _commands;

        private Dictionary<string, LocalCommandInfo> _handlerDict;

        public SlashCommandHandler(DiscordSocketClient client, ulong appId, ulong? guildId)
        {
            _client = client;
            _commands = new List<ApplicationCommandProperties>();
            _handlerDict = new Dictionary<string, LocalCommandInfo>();
            ApplicationId = appId;

            Assembly executable = Assembly.GetEntryAssembly();
            foreach (var type in executable.GetTypes())
            {
                if (typeof(SlashCommand).IsAssignableFrom(type))
                {
                    if (TryGetAttribute<SlashCommandAttribute>(type.GetCustomAttributes(), out var attr))
                    {
                        List<ApplicationCommandOption> parameters = new List<ApplicationCommandOption>();
                        Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();
                        Dictionary<string, PropertyInfo> modalProperties = new Dictionary<string, PropertyInfo>();

                        foreach (var property in type.GetProperties())
                        {
                            if (TryGetAttribute<SlashParameterAttribute>(property.GetCustomAttributes(), out var paramAttr))
                            {
                                List<CommandOptionChoice> choices = null;

                                foreach (var ok in property.GetCustomAttributes())
                                {
                                    if (ok.GetType() == typeof(SlashParameterChoiceAttribute))
                                    {
                                        if (choices == null) choices = new List<CommandOptionChoice>();

                                        var choiceAttr = (SlashParameterChoiceAttribute) ok;

                                        if (choiceAttr.Value.GetType() != typeof(string) && !IsInteger(choiceAttr.Value.GetType()))
                                            throw new InvalidOperationException("All choice values must either be strings or integers");

                                        choices.Add(new CommandOptionChoice()
                                        {
                                            Name = choiceAttr.Name,
                                            Value = choiceAttr.Value
                                        });
                                    }
                                }

                                parameters.Add(new ApplicationCommandOption()
                                {
                                    Name = paramAttr.Name,
                                    Description = paramAttr.Description,
                                    Required = paramAttr.Required ? true : null,
                                    Autocomplete = (property.PropertyType == typeof(string) || IsInteger(property.PropertyType)) && paramAttr.AutoComplete ? true : null,
                                    Choices = choices,
                                    Type = ResolveType(property.PropertyType)
                                });

                                properties[paramAttr.Name] = property;
                            }
                            else if (TryGetAttribute<ModalParameterAttribute>(property.GetCustomAttributes(), out var paramModal)){
                                modalProperties[paramModal.Id] = property;
                            }
                        }

                        TryGetAttribute<SlashCommandCategoryAttribute>(type.GetCustomAttributes(), out var catAttr);

                        string category =  catAttr?.Category;
                        string subcommandgroup =  catAttr?.SubcommandGroup;
                        string sName = "";
                        if(category != null)
                        {
                            sName += category + ".";
                            if(subcommandgroup != null) sName += subcommandgroup + ".";
                        }
                        sName += attr.Name + ".";

                        var handler = _handlerDict[sName.Trim('.')] = new LocalCommandInfo()
                        {
                            HandlerType = type,
                            Delayed = attr.Delayed,
                            Parameters = properties,
                            ModalParameters = modalProperties
                        };

                        if (category != null)
                        {
                            var Opt = new ApplicationCommandOption()
                            {
                                Type = CommandOptionType.SubCommand,
                                Name = attr.Name,
                                Description = attr.Description,
                                Options = parameters
                            };
                            ApplicationCommandOption subOpt = null;
                            if (subcommandgroup != null)
                            {
                                subOpt = new ApplicationCommandOption()
                                {
                                    Type = CommandOptionType.SubCommandGroup,
                                    Name = subcommandgroup,
                                    Description = "SubCommand Group",
                                    Options = new List<ApplicationCommandOption>()
                                    {
                                        Opt
                                    }
                                };
                            }

                            var existing = _commands.Find(c => c.Name == category);
                            var existingSub = existing?.Options.Find(c => c.Name == subcommandgroup);

                            if (existing == null)
                            {
                                _commands.Add(new ApplicationCommandProperties()
                                {
                                    Name = category,
                                    Description = "Category",
                                    Options = new List<ApplicationCommandOption>()
                                    {
                                        subOpt ?? Opt
                                    }
                                });
                            }
                            else
                            {
                                var options = existing.Options.ToList();
                                if(existingSub != null)
                                {
                                    options = existingSub.Options.ToList();
                                    options.Add(Opt);
                                    existingSub.Options = options;
                                } else
                                {
                                    options.Add(subOpt);
                                    existing.Options = options;
                                }
                            }
                        }
                        else
                        {
                            _commands.Add(new ApplicationCommandProperties()
                            {
                                Name = attr.Name,
                                Description = attr.Description,
                                Options = parameters
                            });
                        }
                    }
                    else throw new MissingMemberException("All commands must have a SlashCommand attribute");
                }
            }

            if (guildId.HasValue)
                _client.SetGuildApplicationCommands(appId, guildId.Value, _commands);
            else
                _client.SetGlobalApplicationCommands(appId, _commands);

            client.OnInteraction += Client_OnInteraction;
        }

        private object ResolveObject(ResolvedInteractionData data, ulong id)
        {
            if (data.Roles != null && data.Roles.TryGetValue(id, out var role)) return role;
            else if (data.Members != null && data.Members.TryGetValue(id, out var member)) return member;
            else if (data.Users != null && data.Users.TryGetValue(id, out var user)) return user;
            else throw new Exception(); // what the fuck
        }

        private void Client_OnInteraction(DiscordSocketClient client, DiscordInteractionEventArgs args)
        {
            if ((args.Interaction.Type != DiscordInteractionType.ApplicationCommand && args.Interaction.Type != DiscordInteractionType.ApplicationCommandAutocomplete && args.Interaction.Type != DiscordInteractionType.ModalSubmit) || args.Interaction.ApplicationId != ApplicationId.ToString())
            {
                return;
            }

            if (args.Interaction.Type == DiscordInteractionType.ModalSubmit)
            {
                Handle(args.Interaction.Data.ComponentId, args.Interaction, null);
                return;
            }

            string cmdName = $"{args.Interaction.Data.CommandName}.";
            var opts = args.Interaction.Data.CommandArguments;

            if(opts != null && opts.Count > 0)
            {
                while (opts != null && opts.Count > 0 && opts[0].Options != null)
                {
                    cmdName += opts[0].Name + ".";
                    opts = opts[0].Options;
                }
            }

            Handle(cmdName.Trim('.'), args.Interaction, opts?.ToList());
        }

        private void Handle(string cmdName, DiscordInteraction interaction, List<SlashCommandArgument> arguments)
        {
            var localCommand = _handlerDict[cmdName];

            var handler = (SlashCommand)Activator.CreateInstance(localCommand.HandlerType);
            handler.Prepare(interaction);

            if (arguments != null)
            {
                foreach (var suppliedArg in arguments)
                {
                    var property = localCommand.Parameters[suppliedArg.Name];
                    object value = suppliedArg.Value;

                    switch (suppliedArg.Type)
                    {
                        case CommandOptionType.Channel:
                            value = interaction.Data.Resolved.Channels[ulong.Parse(suppliedArg.Value)];
                            break;
                        case CommandOptionType.Role:
                            value = interaction.Data.Resolved.Roles[ulong.Parse(suppliedArg.Value)];
                            break;
                        case CommandOptionType.User:
                            if (property.PropertyType == typeof(DiscordUser)) value = interaction.Data.Resolved.Users[ulong.Parse(suppliedArg.Value)];
                            else value = interaction.Data.Resolved.Members[ulong.Parse(suppliedArg.Value)];
                            break;
                        case CommandOptionType.Mentionable:
                            value = ResolveObject(interaction.Data.Resolved, ulong.Parse(suppliedArg.Value));
                            break;
                        default:
                            value = Convert.ChangeType(value, property.PropertyType);
                            break;
                    }

                    property.SetValue(handler, value);
                }
            }

            InteractionResponseProperties prop = null;
            if (interaction.Type == DiscordInteractionType.ApplicationCommand)
            {
                prop = handler.Handle();
            } else if (interaction.Type == DiscordInteractionType.ApplicationCommandAutocomplete)
            {
                prop = handler.HandleAutoComplete();
            }
            else if (interaction.Type == DiscordInteractionType.ModalSubmit)
            {
                foreach (RowComponent row in interaction.Data.Components)
                {
                    foreach (MessageInputComponent input in row.Components)
                    {
                        var property = localCommand.ModalParameters[input.Id];
                        object value = input.Value;
                        value = Convert.ChangeType(value, property.PropertyType);
                        property.SetValue(handler, value);
                    }
                }
                prop = handler.HandleModalSubmit();
            }

            if (prop == null)
            {
                return;
            }

            if (localCommand.Delayed)
            {
                interaction.Respond(InteractionCallbackType.DelayedMessage);
                interaction.ModifyResponse(prop);
            }
            else
            {
                interaction.Respond(InteractionCallbackType.RespondWithMessage, prop);
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
