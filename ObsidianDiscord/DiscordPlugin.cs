using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.EventArgs;

using Obsidian.API;
using Obsidian.API.Events;
using Obsidian.API.Plugins;
using Obsidian.API.Plugins.Services;

namespace ObsidianDiscord
{
    [Plugin(Name = "ObsidianDiscord", Version = "0.0.1",
            Authors = "Oliver4888", Description = "",
            ProjectUrl = "https://github.com/oliver4888/ObsidianDiscord")]
    public class DiscordPlugin : PluginBase
    {
        public static DiscordPlugin Instance { get; private set; }

        [Inject]
        public ILogger Logger { get; set; }

        [Inject]
        public IFileReader IFileReader { get; set; }

        [Inject]
        public IFileWriter IFileWriter { get; set; }

        DiscordClient _client;
        IServer _server;
        Config _config;

        public DiscordPlugin() : base()
        {
            if (Instance != null)
                return;

            Instance = this;
        }

        #region Discord Events
        private async Task Discord_Ready(DiscordClient sender, ReadyEventArgs e)
        {
            Logger.LogDebug($"{_client.CurrentUser.Username} ready in {_client.Guilds.Count} guild(s)!");
            await Task.CompletedTask;
        }

        private async Task Discord_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (_server == null || e.Author.IsBot)
                return;

            await _server.BroadcastAsync($"<{e.Author.Username}#{e.Author.Discriminator}> {e.Message.Content}");
        }
        #endregion

        #region Obsidian Events
        public async Task OnLoad(IServer server)
        {
            _server = server;

            _config = ConfigLoader.LoadConfig();

            if (!_config.Enabled)
                return;

            _client = new DiscordClient(new DiscordConfiguration
            {
                Token = _config.Token,
                TokenType = TokenType.Bot
            });

            _client.Ready += Discord_Ready;
            _client.MessageCreated += Discord_MessageCreated;

            _ = Task.Run(async () =>
            {
                await _client.ConnectAsync();
            });

            Logger.Log($"{Info.Name} v{Info.Version} loaded!");

            await Task.CompletedTask;
        }

        public async Task OnPlayerJoin(PlayerJoinEventArgs e)
        {
            if (!(_config.Enabled && _config.JoinLeaveMessages.Enabled))
                return;

            await _client.Guilds[_config.GuildId].Channels[_config.JoinLeaveMessages.ChannelId].SendMessageAsync($"{e.Player.Username} joined the server!");
        }

        public async Task OnPlayerLeave(PlayerLeaveEventArgs e)
        {
            if (!(_config.Enabled && _config.JoinLeaveMessages.Enabled))
                return;

            await _client.Guilds[_config.GuildId].Channels[_config.JoinLeaveMessages.ChannelId].SendMessageAsync($"{e.Player.Username} left the server!");
        }
        #endregion
    }
}
