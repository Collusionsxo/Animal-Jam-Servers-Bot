using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AJ_Servers_Bot
{
    public sealed class Program
    {
        public static DiscordClient Client { get; private set; }
        public static InteractivityExtension Interactivity { get; private set; }
        public static CommandsNextExtension Commands { get; private set; }
        static async Task Main(string[] args)
        {
            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = "YOUR TOKEN HERE",
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };
            Client = new DiscordClient(discordConfig);
            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            });
            Client.Ready += OnClientReady;
            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { "." },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false,
            };
            var slash = Client.UseSlashCommands();
            slash.RegisterCommands<AJ_Servers_Bot.Basic.SlashCommands>();
            Commands = Client.UseCommandsNext(commandsConfig);
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            await Client.UpdateStatusAsync(new DiscordActivity()
            {
                ActivityType = ActivityType.Watching,
                Name = "Animal Jam Servers"
            });
        }
    }
}