using System;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using Obsidian.API;
using Obsidian.API.Events;
using Obsidian.API.Plugins;
using Obsidian.API.Plugins.Services;

using ObsidianDiscord.Configuration;
using ObsidianDiscord.Configuration.Models;

namespace ObsidianDiscord
{
    [Plugin(Name = "ObsidianDiscord", Version = "0.0.1",
            Authors = "Oliver4888", Description = "A Discord chat bridge for Minecraft Obsidian servers.",
            ProjectUrl = "https://github.com/oliver4888/ObsidianDiscord")]
    public class DiscordPlugin : PluginBase
    {
        internal static DiscordPlugin Instance { get; private set; }

        [Inject]
        public ILogger Logger { get; set; }

        [Inject]
        public IFileReader IFileReader { get; set; }

        [Inject]
        public IFileWriter IFileWriter { get; set; }

        readonly ConfigLoader _configLoader = new ConfigLoader();

        DiscordClient _client;
        IServer _server;
        PluginConfig _config;

        Timer _statusTimer;
        DiscordActivity _discordStatus = new DiscordActivity();

        public DiscordPlugin() : base()
        {
            if (Instance != null)
                return;

            Instance = this;
        }

        #region Discord
        private void SetStatusMessage() => _discordStatus.Name = string.Format(_config.BotStatus.Template, _server.Players.Count(), _server.Configuration.MaxPlayers, _server.TPS);

        private async Task UpdateStatus()
        {
            SetStatusMessage();

            try
            {
                await _client.UpdateStatusAsync(_discordStatus);
            }
            catch (Exception ex)
            {
                Logger.LogError($"UpdateStatus: {ex.Message}");
            }
        }

        private async Task Discord_Ready(DiscordClient sender, ReadyEventArgs e)
        {
            Logger.LogDebug($"{_client.CurrentUser.Username} ready in {_client.Guilds.Count} guild(s)!");
            await Task.CompletedTask;
        }

        private async Task Discord_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (e.Author.IsBot)
                return;

            await _server.BroadcastAsync($"<{e.Author.Username}#{e.Author.Discriminator}> {e.Message.Content}");
        }
        #endregion

        #region Obsidian Events
        public async Task OnLoad(IServer server)
        {
            _server = server;

            _config = await _configLoader.LoadConfig<PluginConfig>();

            if (!_config.Enabled)
                return;

            SetStatusMessage();

            _statusTimer = new Timer(_config.BotStatus.Interval);
            _statusTimer.Elapsed += async (sender, e) => await UpdateStatus();
            _statusTimer.Start();

            _client = new DiscordClient(new DiscordConfiguration
            {
                Token = _config.Token,
                TokenType = TokenType.Bot
            });

            _client.Ready += Discord_Ready;

            if (_server != null && _config.ChatSync.Enabled)
                _client.MessageCreated += Discord_MessageCreated;

            _ = Task.Run(async () =>
            {
                await _client.ConnectAsync(_discordStatus);
            });

            Logger.Log($"{Info.Name} v{Info.Version} loaded!");

            await Task.CompletedTask;
        }

        public async Task OnPlayerJoin(PlayerJoinEventArgs e)
        {
            if (!(_config.Enabled && _config.JoinLeaveMessages.Enabled))
                return;

            _ = Task.Run(async () =>
            {
                if (_config.ChatSync.Enabled)
                    await _client.Guilds[_config.GuildId].Channels[_config.JoinLeaveMessages.ChannelId].SendMessageAsync($"{e.Player.Username} joined the server!");
            });
            await Task.CompletedTask;
        }

        public async Task OnPlayerLeave(PlayerLeaveEventArgs e)
        {
            if (!(_config.Enabled && _config.JoinLeaveMessages.Enabled))
                return;

            _ = Task.Run(async () =>
            {
                if (_config.ChatSync.Enabled)
                    await _client.Guilds[_config.GuildId].Channels[_config.JoinLeaveMessages.ChannelId].SendMessageAsync($"{e.Player.Username} left the server!");
            });
            await Task.CompletedTask;
        }

        public async Task OnIncomingChatMessage(IncomingChatMessageEventArgs e)
        {
            _ = Task.Run(async () =>
            {
                if (_config.ChatSync.Enabled)
                    await _client.Guilds[_config.GuildId].Channels[_config.JoinLeaveMessages.ChannelId].SendMessageAsync($"{e.Player.Username}: {e.Message}");
            });
            await Task.CompletedTask;
        }
        #endregion
    }
}
