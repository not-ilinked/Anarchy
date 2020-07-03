using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Discord.Commands
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        public string Prefix { get; private set; }
        private readonly List<KeyValuePair<DiscordCommand, Type>> _commands;
        public List<DiscordCommand> Commands { get; private set; }

        public delegate void MissingHandler(object sender, MissingParameterEventArgs e);
        public event MissingHandler OnMissingParameter;

        public delegate void InvalidHandler(object sender, InvalidParameterEventArgs e);
        public event InvalidHandler OnInvalidParameter;

        public CommandHandler(string prefix, DiscordSocketClient client)
        {
            _client = client;

            Prefix = prefix;
            client.OnMessageReceived += Client_OnMessageReceived;

            _commands = new List<KeyValuePair<DiscordCommand, Type>>();
            Commands = new List<DiscordCommand>();

            Assembly executable = Assembly.GetEntryAssembly();

            foreach (var type in executable.GetTypes())
            {
                try
                {
                    CommandAttribute attribute = (CommandAttribute)type.GetCustomAttributes().First(a => a.GetType() == typeof(CommandAttribute));

                    if (!type.GetInterfaces().Contains(typeof(ICommand)))
                        throw new NotImplementedException("All Anarchy command handlers must inherit ICommand");

                    DiscordCommand cmd = new DiscordCommand(attribute.Command, FindArgumentProperties(type), attribute.Description);

                    _commands.Add(new KeyValuePair<DiscordCommand, Type>(cmd, type));

                    Commands.Add(cmd);
                }
                catch { }
            }
        }


        private void Client_OnMessageReceived(DiscordSocketClient client, MessageEventArgs args)
        {
            if (args.Message.Content.StartsWith(Prefix))
            {
                string[] contents = args.Message.Content.Split(' ');

                if (Prefix.Length < contents[0].Length)
                {
                    string command = new string(contents[0].Skip(Prefix.Length).ToArray());

                    if (TryGetCommand(command, out KeyValuePair<DiscordCommand, Type> cmd))
                    {
                        object handlerInstance = Activator.CreateInstance(cmd.Value);

                        string[] arguments = contents.Skip(1).ToArray();
                        var expectedParameters = cmd.Key._parameters;

                        for (int i = 0; i < expectedParameters.Count; i++)
                        {
                            if (arguments.Length - 1 < i)
                            {
                                if (expectedParameters[i].Value.Optional)
                                    break;
                                else
                                {
                                    OnMissingParameter?.Invoke(this, new MissingParameterEventArgs(cmd.Key, expectedParameters[i].Value));

                                    return;
                                }
                            }

                            try
                            {
                                object translated = null;

                                string parsedArg = arguments[i];

                                if (parsedArg.StartsWith("<") && parsedArg.EndsWith(">"))
                                {
                                    parsedArg = new string(parsedArg.Where(char.IsDigit).ToArray());

                                    if (arguments[i].StartsWith("<#") && expectedParameters[i].Key.PropertyType == typeof(MinimalTextChannel)) // channels have a minimal class :D
                                        translated = new MinimalTextChannel(ulong.Parse(parsedArg)).SetClient(_client);
                                }
                                else if (expectedParameters[i].Key.PropertyType == typeof(string) && i == expectedParameters.Count - 1) // if the last expected arg is a string, we want to send the rest of the users message
                                    translated = string.Join(" ", arguments.Skip(i));

                                if (translated == null)
                                    translated = Convert.ChangeType(parsedArg, expectedParameters[i].Key.PropertyType);

                                expectedParameters[i].Key.SetValue(handlerInstance, translated);
                            }
                            catch
                            {
                                OnInvalidParameter?.Invoke(this, new InvalidParameterEventArgs(cmd.Key, expectedParameters[i].Value, arguments[i]));

                                return;
                            }
                        }

                        cmd.Value.GetMethod("Execute").Invoke(handlerInstance, new object[] { client, args.Message });
                    }
                }
            }
        }

        private bool TryGetCommand(string command, out KeyValuePair<DiscordCommand, Type> commandHandler)
        {
            foreach (var cmd in _commands)
            {
                if (cmd.Key.Command == command)
                {
                    commandHandler = cmd;

                    return true;
                }
            }

            commandHandler = new KeyValuePair<DiscordCommand, Type>();

            return false;
        }


        private List<KeyValuePair<PropertyInfo, ParameterAttribute>> FindArgumentProperties(Type type)
        {
            var args = new List<KeyValuePair<PropertyInfo, ParameterAttribute>>();

            foreach (var property in type.GetProperties())
            {
                foreach (var attribute in property.GetCustomAttributes())
                {
                    if (attribute.GetType() == typeof(ParameterAttribute))
                    {
                        args.Add(new KeyValuePair<PropertyInfo, ParameterAttribute>(property, (ParameterAttribute)attribute));

                        break;
                    }
                }
            }

            return args;
        }
    }
}
