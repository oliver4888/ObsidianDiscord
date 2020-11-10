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

        readonly ConfigManager _configManager = new ConfigManager();

        DiscordClient _client;
        IServer _server;

        PluginConfig _config;
        WebhookConfig _webhook;

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

            _ = Task.Run(async () =>
            {
                if (_config.ChatSync.UseWebhooks)
                {
                    if (_webhook.WebhookId == 0)
                    {
                        DiscordChannel channel = await _client.GetChannelAsync(_config.ChatSync.ChannelId);
                        _webhook.Webhook = await channel.CreateWebhookAsync("ObsidianDiscord_Webhook");
                        _webhook.WebhookId = _webhook.Webhook.Id;
                        await _configManager.SaveConfig(_webhook);
                    }
                    else
                        _webhook.Webhook = await _client.GetWebhookAsync(_webhook.WebhookId);
                }
            });

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

            _config = await _configManager.LoadConfig<PluginConfig>();
            _webhook = await _configManager.LoadConfig<WebhookConfig>();

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
        }

        public async Task OnPlayerJoin(PlayerJoinEventArgs e)
        {
            if (!(_config.Enabled && _config.JoinLeaveMessages.Enabled))
                return;

            _ = Task.Run(async () =>
            {
                if (_config.ChatSync.Enabled)
                {
                    string message = string.Format(_config.JoinLeaveMessages.JoinMessageTemplate, e.Player.Username);
                    await (await _client.GetChannelAsync(_config.JoinLeaveMessages.ChannelId)).SendMessageAsync(message);
                }
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
                {
                    string message = string.Format(_config.JoinLeaveMessages.LeaveMessageTemplate, e.Player.Username);
                    await (await _client.GetChannelAsync(_config.JoinLeaveMessages.ChannelId)).SendMessageAsync(message);
                }
            });
            await Task.CompletedTask;
        }

        public async Task OnIncomingChatMessage(IncomingChatMessageEventArgs e)
        {
            _ = Task.Run(async () =>
            {
                if (_config.ChatSync.Enabled)
                {
                    if (_config.ChatSync.UseWebhooks)
                        await _webhook.Webhook.ExecuteAsync(new DiscordWebhookBuilder
                        {
                            AvatarUrl = $"https://minotar.net/avatar/{e.Player.Uuid.ToString().Replace("-", "")}",
                            Username = e.Player.Username,
                            Content = e.Message
                        });
                    else
                    {
                        string message = string.Format(_config.ChatSync.FallbackMessageTemplate, e.Player.Username, e.Message);
                        await (await _client.GetChannelAsync(_config.JoinLeaveMessages.ChannelId)).SendMessageAsync(message);
                    }
                }
            });
            await Task.CompletedTask;
        }
        #endregion
    }
}
