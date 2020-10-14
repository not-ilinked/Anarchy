using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Discord.Commands
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        public string Prefix { get; private set; }
        public Dictionary<string, DiscordCommand> Commands { get; private set; }

        internal CommandHandler(string prefix, DiscordSocketClient client)
        {
            _client = client;

            Prefix = prefix;
            client.OnMessageReceived += Client_OnMessageReceived;

            Assembly executable = Assembly.GetEntryAssembly();

            Commands = new Dictionary<string, DiscordCommand>();
            foreach (var type in executable.GetTypes())
            {
                if (typeof(CommandBase).IsAssignableFrom(type) && TryGetAttribute(type.GetCustomAttributes(), out CommandAttribute attr))
                    Commands.Add(attr.Name, new DiscordCommand(type, attr));
            }
        }


        private void Client_OnMessageReceived(DiscordSocketClient client, MessageEventArgs args)
        {
            if (args.Message.Content.StartsWith(Prefix))
            {
                List<string> parts = args.Message.Content.Split(' ').ToList();

                if (Commands.TryGetValue(parts[0].Substring(Prefix.Length), out DiscordCommand command))
                {
                    parts.RemoveAt(0);

                    CommandBase inst = (CommandBase)Activator.CreateInstance(command.Type);
                    inst.Prepare(_client, args.Message);

                    for (int i = 0; i < command.Parameters.Count; i++)
                    {
                        var param = command.Parameters[i];

                        if (i < parts.Count)
                        {
                            try
                            {
                                object value;

                                if (param.Property.PropertyType == typeof(string) && i == command.Parameters.Count - 1)
                                    value = string.Join(" ", parts.Skip(i - 1));
                                else if (args.Message.Guild != null && parts[i].StartsWith("<") && parts[i].EndsWith(">"))
                                    value = ParseFormatted(param.Property.PropertyType, parts[i]);
                                else
                                    value = parts[i];

                                if (!param.Property.PropertyType.IsAssignableFrom(value.GetType()))
                                    value = Convert.ChangeType(value, param.Property.PropertyType);

                                param.Property.SetValue(inst, value);
                            }
                            catch (Exception ex)
                            {
                                inst.HandleError(param.Name, parts[i], ex);

                                return;
                            }
                        }
                        else if (param.Optional)
                            break;
                        else
                        {
                            inst.HandleError(param.Name, null, new ArgumentNullException("Too few arguments provided"));

                            return;
                        }
                    }

                    inst.Execute();
                }
            }
        }

        // https://discord.com/developers/docs/reference#message-formatting
        private object ParseFormatted(Type expectedType, string formatted)
        {
            ulong id = 0;
            string type = null;

            for (int i = 0; i < formatted.Length; i++)
            {
                if (char.IsDigit(formatted[i]))
                {
                    id = ulong.Parse(formatted.Substring(i, formatted.Length - 1 - i));
                    type = formatted.Substring(1, i - 1);
                    break;
                }
            }

            if (type != null)
            {
                if (type == "#")
                {
                    if (_client.Config.Cache)
                        return _client.GetChannel(id);
                    else if (expectedType.IsAssignableFrom(typeof(MinimalChannel)))
                        return new MinimalTextChannel(id);
                }
                else if (_client.Config.Cache)
                {
                    if (type == "@&" && expectedType == typeof(DiscordRole))
                        return _client.GetGuildRole(id);
                    else if (type.StartsWith(":") && expectedType == typeof(DiscordEmoji))
                        return _client.GetGuildEmoji(id);
                }

                return id;
            }
            else
                throw new ArgumentException("Failed to parse formatted string");
        }

        internal static bool TryGetAttribute<TAttr>(IEnumerable<object> attributes, out TAttr attr) where TAttr : Attribute
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
