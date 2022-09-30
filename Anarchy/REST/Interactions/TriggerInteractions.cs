using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;
using Discord.Gateway;

namespace Discord
{
    public class TriggerInteractions
    {
        private static ulong BotID = 0;
        private static ulong GuildID = 0;
        private static ulong ChannelID = 0;
        private static List<ApplicationCommand> listSlashCommand;
        private static DiscordClient Client;

        public TriggerInteractions(DiscordClient client, ulong ulChannelID, ulong ulBotID)
        {
            //code
            BotID = ulBotID;

            ChannelID = ulChannelID;

            Client = client;

            DiscordChannel channel = Client.GetChannel(ulChannelID);
            if (channel.InGuild)
            {
                GuildChannel guildChannel = (GuildChannel) channel;
                GuildID = guildChannel.Guild.Id;
            }

            ////Get List Slash Commands
            if (BotID > 0)
            {
                 listSlashCommand = Client.SearchApplicationCommands(BotID, ChannelID);
            }

        }

        public void ClickButton(DiscordMessage msg, string sButtonLabel)
        {
            if (Client == null || msg.Components == null || GuildID == 0 || ChannelID == 0)
            {
                return;
            }

            string sCustomID = "";

            foreach (var comp1 in msg.Components)
            {
                var comp = (RowComponent) comp1;

                foreach (var c in comp.Components)
                {
                    if (c.Type == MessageComponentType.Button)
                    {
                        var btn = (ButtonComponent) c;
                        if (btn.Text == sButtonLabel)
                        {
                            sCustomID = btn.Id;
                        }
                    }
                }
            }

            if (sCustomID == "")
            {
                return;
            }

            var dataInteraction = new DiscordInteraction
            {
                Type = DiscordInteractionType.MessageComponent,
                ApplicationId = BotID.ToString(),
                GuildId = GuildID.ToString(),
                ChannelId = ChannelID.ToString(),
                MessageId = msg.Id.ToString(),
                SessionId = RandomString(32),
                Data = new DiscordInteractionData
                {
                    ComponentType = MessageComponentType.Button,
                    ComponentId = sCustomID.ToString()
                }
            };

            Client.SendInteractionAsync(dataInteraction);
        }

        public void SelectValue(DiscordMessage msg, string sSelectLabel)
        {
            if (Client == null || msg.Components == null || GuildID == 0 || ChannelID == 0)
            {
                return;
            }

            string sCustomID = "";
            string sValue = "";


            foreach (var comp1 in msg.Components)
            {
                var comp = (RowComponent) comp1;
                foreach (var c in comp.Components)
                {
                    if (c.Type == MessageComponentType.Select)
                    {
                        var select = (SelectMenuComponent) c;
                        foreach (var slOption in select.Options)
                        {
                            if (slOption.Text == sSelectLabel)
                            {
                                sValue = slOption.Value;
                            }
                        }
                        sCustomID = select.Id;
                    }
                }
            }

            if (sValue == "")
            {
                return;
            }

            var dataInteraction = new DiscordInteraction
            {
                Type = DiscordInteractionType.MessageComponent,
                ApplicationId = BotID.ToString(),
                GuildId = GuildID.ToString(),
                ChannelId = ChannelID.ToString(),
                MessageId = msg.Id.ToString(),
                SessionId = RandomString(32),
                Data = new DiscordInteractionData
                {
                    ComponentType = MessageComponentType.Select,
                    ComponentId = sCustomID.ToString(),
                    SelectMenuValues = new string[]
                    {
                        sValue
                    }
                }
            };

            Client.SendInteractionAsync(dataInteraction);
        }

        public void SendSlash(string sCmd, params string[] args)
        {
            if (Client == null || string.IsNullOrEmpty(sCmd) || GuildID == 0 || ChannelID == 0)
            {
                return;
            }

            var listDataSlash = GetDataSlash(listSlashCommand, sCmd, args);

            if (listDataSlash == null)
            {
                return;
            }

            var dataInteraction = new DiscordInteraction
            {
                Type = DiscordInteractionType.ApplicationCommand,
                ApplicationId = BotID.ToString(),
                GuildId = GuildID.ToString(),
                ChannelId = ChannelID.ToString(),
                SessionId = RandomString(32),
                Data = listDataSlash
            };

            Client.SendInteractionAsync(dataInteraction);
        }

        private static dynamic GetDataSlash(IReadOnlyList<ApplicationCommand> ListSlash, string sName, params string[] args)
        {
            foreach (var command in ListSlash)
            {
                var ArrCmd = Array.Empty<string>();
                var cmdName = sName;
                if (sName.Contains(' '))
                {
                    ArrCmd = sName.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    cmdName = ArrCmd[0];
                }

                if (command.Name == cmdName)
                {
                    //Get main command
                    var SlashData = new DiscordInteractionData
                    {
                        ComponentType = MessageComponentType.Row,
                        Version = command.Version.ToString(),
                        CommandId = command.Id.ToString(),
                        CommandName = command.Name,
                        CommandArguments = new List<SlashCommandArgument>()
                    };

                    var SlashDataOptions = SlashData.CommandArguments;
                    var options = command.Options;

                    if (ArrCmd.Length > 0)
                    {
                        ArrCmd = ArrCmd.Skip(1).ToArray();
                        var iCheck = 0;
                        while (ArrCmd.Length > 0 && iCheck < 5)
                        {
                            if (options != null)
                            {
                                foreach (var listOpt in options)
                                {
                                    if (listOpt.Name == ArrCmd[0])
                                    {
                                        var cOptionSlash = new SlashCommandArgument
                                        {
                                            Type = listOpt.Type,
                                            Name = listOpt.Name
                                        };

                                        options = listOpt.Options;
                                        ArrCmd = ArrCmd.Skip(1).ToArray();
                                        if (options != null || ArrCmd.Length > 0)
                                        {
                                            cOptionSlash.Options = new List<SlashCommandArgument>();
                                        }
                                        SlashDataOptions.Add(cOptionSlash);
                                        if (options != null || ArrCmd.Length > 0)
                                        {
                                            SlashDataOptions = cOptionSlash.Options;
                                        }
                                        break;
                                    }
                                }
                            }
                            iCheck++;
                        }
                    }

                    if (options != null)
                    {
                        var iCount = 0;
                        foreach (var option in options)
                        {
                            if (iCount < args.Length)
                            {
                                var cOptionSlash = new SlashCommandArgument
                                {
                                    Type = option.Type,
                                    Name = option.Name,
                                    Value = DynamicValue(option.Type, args[iCount])
                                };
                                SlashDataOptions.Add(cOptionSlash);
                            }
                        }
                    }
                    return SlashData;
                }
            }
            return null;
        }

        private static dynamic DynamicValue(CommandOptionType type, string sValue)
        {
            return type switch
            {
                CommandOptionType.Boolean => Convert.ToBoolean(sValue),
                CommandOptionType.Integer => Convert.ToInt32(sValue),
                CommandOptionType.Number => Convert.ToDouble(sValue),
                _ => sValue,
            };
        }

        private static string RandomString(int length)
        {
            Random random = new();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var builder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var c = pool[random.Next(0, pool.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }
    }
}