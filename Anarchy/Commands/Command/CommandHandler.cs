using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

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
                {
                    if (Commands.ContainsKey(attr.Name))
                        throw new InvalidOperationException($"More than 1 command has the name \"{attr.Name}\"");
                    else
                        Commands.Add(attr.Name, new DiscordCommand(type, attr));
                }
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
                                    value = string.Join(" ", parts.Skip(i));
                                else if (parts[i].StartsWith("<") && parts[i].EndsWith(">"))
                                    value = ParseReference(args.Message, param.Property.PropertyType, parts[i]);
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
        private object ParseReference(DiscordMessage message, Type expectedType, string reference)
        {
            string value = reference.Substring(1, reference.Length - 2);

            // Get the object's ID (always last thing in the sequence)

            MatchCollection matches = Regex.Matches(value, @"\d{18,}");
            if (matches.Count > 0) 
            {
                Match match = matches[matches.Count - 1];

                if (match.Index + match.Length == value.Length)
                {
                    ulong anyId = ulong.Parse(match.Value);

                    string forSpecific = value.Substring(0, match.Index);

                    if (expectedType.IsAssignableFrom(typeof(MinimalChannel)))
                    {
                        if (forSpecific == "#")
                        {
                            if (expectedType.IsAssignableFrom(typeof(DiscordChannel)))
                            {
                                if (_client.Config.Cache)
                                    return _client.GetChannel(anyId);
                                else
                                    throw new InvalidOperationException("Caching must be enabled to parse DiscordChannels");
                            }
                            else
                                return new MinimalTextChannel(anyId).SetClient(_client);
                        }
                        else
                            throw new ArgumentException("Invalid reference type");
                    }
                    else if (expectedType == typeof(DiscordRole))
                    {
                        if (forSpecific.StartsWith("@&"))
                        {
                            if (_client.Config.Cache)
                                return _client.GetGuildRole(anyId);
                            else
                                throw new InvalidOperationException("Caching must be enabled to parse DiscordChannels");
                        }
                        else
                            throw new ArgumentException("Invalid reference type");
                    }
                    else if (expectedType.IsAssignableFrom(typeof(PartialEmoji)))
                    {
                        if (Regex.IsMatch(forSpecific, @"a?:\w+:"))
                        {
                            string[] split = forSpecific.Split(':');

                            bool animated = split[0] == "a";
                            string name = split[1];

                            if (expectedType == typeof(DiscordEmoji))
                            {
                                if (_client.Config.Cache)
                                    return _client.GetGuildEmoji(anyId);
                                else
                                    throw new InvalidOperationException("Caching must be enabled to parase DiscordEmojis");
                            }
                            else
                                return new PartialEmoji(anyId, name, animated).SetClient(_client);
                        }
                        else
                            throw new ArgumentException("Invalid reference type");
                    }
                    else if (expectedType == typeof(DiscordUser))
                    {
                        if (forSpecific == "@" || forSpecific == "@!")
                            return message.Mentions.First(m => m.Id == anyId);
                    }
                    else
                        return anyId;
                }
            }
            
            throw new ArgumentException("Invalid reference");
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
