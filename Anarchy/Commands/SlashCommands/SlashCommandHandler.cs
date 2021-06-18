using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Commands
{
    internal class SlashCommandHandler
    {
        private class LocalCommandInfo
        {
            public Type HandlerType { get; set; }
            public Dictionary<string, PropertyInfo> Parameters { get; set; }
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

                                        var choiceAttr = (SlashParameterChoiceAttribute)ok;

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
                                    Required = paramAttr.Required,
                                    Choices = choices,
                                    Type = ResolveType(property.PropertyType)
                                });

                                properties[paramAttr.Name] = property;
                            }
                        }

                        _commands.Add(new ApplicationCommandProperties()
                        {
                            Name = attr.Name,
                            Description = attr.Description,
                            Options = parameters
                        });

                        _handlerDict[attr.Name] = new LocalCommandInfo()
                        {
                            HandlerType = type,
                            Delayed = attr.Delayed,
                            Parameters = properties
                        };
                    }
                    else throw new MissingMemberException("All commands must have a SlashCommand attribute");
                }
            }

            if (guildId.HasValue) _client.HttpClient.PutAsync($"/applications/{appId}/guilds/{guildId.Value}/commands", _commands).GetAwaiter().GetResult();
            else _client.SetGlobalApplicationCommands(appId, _commands);

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
            if (args.Interaction.Type == DiscordInteractionType.ApplicationCommand && args.Interaction.ApplicationId == ApplicationId)
            {
                foreach (var cmd in _commands)
                {
                    if (cmd.Name == args.Interaction.Data.CommandName)
                    {
                        var localCommand = _handlerDict[cmd.Name];

                        var handler = (SlashCommand)Activator.CreateInstance(localCommand.HandlerType);
                        handler.Prepare(args.Interaction);

                        if (args.Interaction.Data.CommandArguments != null)
                        {
                            foreach (var suppliedArg in args.Interaction.Data.CommandArguments)
                            {
                                foreach (var param in cmd.Options)
                                {
                                    if (param.Name == suppliedArg.Name)
                                    {
                                        var property = localCommand.Parameters[param.Name];
                                        object value = suppliedArg.Value;

                                        switch (param.Type)
                                        {
                                            case CommandOptionType.Channel:
                                                value = args.Interaction.Data.Resolved.Channels[ulong.Parse(suppliedArg.Value)];
                                                break;
                                            case CommandOptionType.Role:
                                                value = args.Interaction.Data.Resolved.Roles[ulong.Parse(suppliedArg.Value)];
                                                break;
                                            case CommandOptionType.User:
                                                if (property.PropertyType == typeof(DiscordUser)) value = args.Interaction.Data.Resolved.Users[ulong.Parse(suppliedArg.Value)];
                                                else value = args.Interaction.Data.Resolved.Members[ulong.Parse(suppliedArg.Value)];
                                                break;
                                            case CommandOptionType.Mentionable:
                                                value = ResolveObject(args.Interaction.Data.Resolved, ulong.Parse(suppliedArg.Value));
                                                break;
                                            default:
                                                value = Convert.ChangeType(value, property.PropertyType);
                                                break;
                                        }

                                        property.SetValue(handler, value);

                                        break;
                                    }
                                }
                            }
                        }

                        if (localCommand.Delayed)
                        {
                            try
                            {
                                args.Interaction.Respond(InteractionCallbackType.DelayedMessage);
                                args.Interaction.ModifyResponse(handler.Handle());
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
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
