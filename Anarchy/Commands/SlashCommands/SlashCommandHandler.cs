using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Discord.Commands
{
    internal class SlashCommandHandler
    {
        private class LocalCommandInfo
        {
            public Type HandlerType { get; set; }
            public Dictionary<string, PropertyInfo> Parameters { get; set; }
            public bool Delayed { get; set; }
            public bool Ephemeral { get; set; }
        }

        private readonly DiscordSocketClient _client;
        public ulong ApplicationId { get; private set; }
        private readonly List<ApplicationCommandProperties> _commands;

        private readonly Dictionary<string, LocalCommandInfo> _handlerDict;

        public SlashCommandHandler(DiscordSocketClient client, ulong appId, ulong? guildId)
        {
            _client = client;
            _commands = new List<ApplicationCommandProperties>();
            _handlerDict = new Dictionary<string, LocalCommandInfo>();
            ApplicationId = appId;

            Assembly executable = Assembly.GetEntryAssembly();
            foreach (Type type in executable.GetTypes())
            {
                if (typeof(SlashCommand).IsAssignableFrom(type))
                {
                    if (TryGetAttribute<SlashCommandAttribute>(type.GetCustomAttributes(), out SlashCommandAttribute attr))
                    {
                        List<ApplicationCommandOption> parameters = new List<ApplicationCommandOption>();
                        Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();

                        foreach (PropertyInfo property in type.GetProperties())
                        {
                            if (TryGetAttribute<SlashParameterAttribute>(property.GetCustomAttributes(), out SlashParameterAttribute paramAttr))
                            {
                                List<CommandOptionChoice> choices = null;

                                foreach (Attribute ok in property.GetCustomAttributes())
                                {
                                    if (ok.GetType() == typeof(SlashParameterChoiceAttribute))
                                    {
                                        if (choices == null)
                                        {
                                            choices = new List<CommandOptionChoice>();
                                        }

                                        SlashParameterChoiceAttribute choiceAttr = (SlashParameterChoiceAttribute)ok;

                                        if (choiceAttr.Value.GetType() != typeof(string) && !IsInteger(choiceAttr.Value.GetType()))
                                        {
                                            throw new InvalidOperationException("All choice values must either be strings or integers");
                                        }

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
                                    Required = paramAttr.Required,
                                    Choices = choices,
                                    Type = ResolveType(property.PropertyType)
                                });

                                properties[paramAttr.Name] = property;
                            }
                        }

                        string category = TryGetAttribute<SlashCommandCategoryAttribute>(type.GetCustomAttributes(), out SlashCommandCategoryAttribute catAttr) ? catAttr.Category : null;

                        LocalCommandInfo handler = _handlerDict[category == null ? attr.Name : $"{category}.{attr.Name}"] = new LocalCommandInfo()
                        {
                            HandlerType = type,
                            Delayed = attr.Delayed,
                            Ephemeral = attr.Ephemeral,
                            Parameters = properties
                        };

                        if (category != null)
                        {
                            ApplicationCommandProperties existing = _commands.Find(c => c.Name == category);

                            if (existing == null)
                            {
                                _commands.Add(new ApplicationCommandProperties()
                                {
                                    Name = category,
                                    Description = "choose an action",
                                    Options = new List<ApplicationCommandOption>()
                                    {
                                        new ApplicationCommandOption()
                                        {
                                            Type = CommandOptionType.SubCommand,
                                            Name = attr.Name,
                                            Description = attr.Description,
                                            Options = parameters
                                        }
                                    }
                                });
                            }
                            else
                            {
                                List<ApplicationCommandOption> options = existing.Options.ToList();

                                options.Add(new ApplicationCommandOption()
                                {
                                    Type = CommandOptionType.SubCommand,
                                    Name = attr.Name,
                                    Description = attr.Description,
                                    Options = parameters
                                });

                                existing.Options = options;
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
                    else
                    {
                        throw new MissingMemberException("All commands must have a SlashCommand attribute");
                    }
                }
            }

            if (guildId.HasValue)
            {
                _client.HttpClient.PutAsync($"/applications/{appId}/guilds/{guildId.Value}/commands", _commands).GetAwaiter().GetResult();
            }
            else
            {
                _client.SetGlobalApplicationCommands(appId, _commands);
            }

            client.OnInteraction += Client_OnInteraction;
        }

        private object ResolveObject(ResolvedInteractionData data, ulong id)
        {
            if (data.Roles != null && data.Roles.TryGetValue(id, out DiscordRole role))
            {
                return role;
            }
            else if (data.Members != null && data.Members.TryGetValue(id, out GuildMember member))
            {
                return member;
            }
            else if (data.Users != null && data.Users.TryGetValue(id, out DiscordUser user))
            {
                return user;
            }
            else
            {
                throw new Exception(); // what the fuck
            }
        }

        private void Client_OnInteraction(DiscordSocketClient client, DiscordInteractionEventArgs args)
        {
            if (args.Interaction.Type == DiscordInteractionType.ApplicationCommand && args.Interaction.ApplicationId == ApplicationId)
            {
                foreach (ApplicationCommandProperties cmd in _commands)
                {
                    if (cmd.Name == args.Interaction.Data.CommandName)
                    {
                        if (cmd.Options.Count > 0 && cmd.Options[0].Type == CommandOptionType.SubCommand)
                        {
                            SlashCommandArgument subCommand = args.Interaction.Data.CommandArguments[0];
                            Handle($"{cmd.Name}.{subCommand.Name}", args.Interaction, subCommand.Options == null ? null : subCommand.Options.ToList());
                        }
                        else
                        {
                            Handle(cmd.Name, args.Interaction, args.Interaction.Data.CommandArguments == null ? new List<SlashCommandArgument>() : args.Interaction.Data.CommandArguments.ToList());
                        }

                        break;
                    }
                }
            }
        }

        private void Handle(string cmdName, DiscordInteraction interaction, List<SlashCommandArgument> arguments)
        {
            LocalCommandInfo localCommand = _handlerDict[cmdName];

            SlashCommand handler = (SlashCommand)Activator.CreateInstance(localCommand.HandlerType);
            handler.Prepare(interaction);

            if (arguments != null)
            {
                foreach (SlashCommandArgument suppliedArg in arguments)
                {
                    PropertyInfo property = localCommand.Parameters[suppliedArg.Name];
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
                            if (property.PropertyType == typeof(DiscordUser))
                            {
                                value = interaction.Data.Resolved.Users[ulong.Parse(suppliedArg.Value)];
                            }
                            else
                            {
                                value = interaction.Data.Resolved.Members[ulong.Parse(suppliedArg.Value)];
                            }

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

            try
            {
                if (localCommand.Delayed)
                {
                    interaction.Respond(InteractionCallbackType.DelayedMessage, new InteractionResponseProperties() { Ephemeral = localCommand.Ephemeral });
                    interaction.ModifyResponse(handler.Handle());
                }
                else
                {
                    InteractionResponseProperties resp = handler.Handle();
                    resp.Ephemeral = localCommand.Ephemeral;
                    interaction.Respond(InteractionCallbackType.RespondWithMessage, resp);
                }
            }
            catch { }
        }

        private bool IsInteger(Type type)
        {
            return type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong);
        }

        private CommandOptionType ResolveType(Type type)
        {
            if (type == typeof(string))
            {
                return CommandOptionType.String;
            }

            if (type == typeof(bool))
            {
                return CommandOptionType.Boolean;
            }

            if (IsInteger(type))
            {
                return CommandOptionType.Integer;
            }

            if (type == typeof(DiscordChannel))
            {
                return CommandOptionType.Channel;
            }

            if (type == typeof(DiscordRole))
            {
                return CommandOptionType.Role;
            }

            if (type == typeof(DiscordUser) || type == typeof(GuildMember))
            {
                return CommandOptionType.User;
            }

            if (typeof(IMentionable).IsAssignableFrom(type))
            {
                return CommandOptionType.Mentionable;
            }
            else
            {
                throw new InvalidOperationException("Unexpected parameter type encountered");
            }
        }

        private static bool TryGetAttribute<TAttr>(IEnumerable<object> attributes, out TAttr attr) where TAttr : Attribute
        {
            foreach (object attribute in attributes)
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
