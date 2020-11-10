using ObsidianDiscord.Configuration.Attributes;

namespace ObsidianDiscord.Configuration.Models
{
    [ConfigFileName("ObsidianDiscord.json")]
    public class PluginConfig
    {
        public bool Enabled { get; set; } = false;
        public string Token { get; set; }
        public ulong GuildId { get; set; }

        public JoinLeaveMessageConfig JoinLeaveMessages { get; set; } = new JoinLeaveMessageConfig();
        public ChatSyncConfig ChatSync { get; set; } = new ChatSyncConfig();
        public StatusConfig BotStatus { get; set; } = new StatusConfig();
    }

    public class JoinLeaveMessageConfig
    {
        public bool Enabled { get; set; } = false;
        public ulong ChannelId { get; set; }
        public string JoinMessageTemplate { get; set; } = "{0} joined the server!";
        public string LeaveMessageTemplate { get; set; } = "{1} left the server!";
    }

    public class ChatSyncConfig
    {
        public bool Enabled { get; set; } = false;
        public ulong ChannelId { get; set; }
        public bool UseWebhooks { get; set; } = true;
        public string FallbackMessageTemplate { get; set; } = "{0}: {1}";
    }

    public class StatusConfig
    {
        public bool Enabled { get; set; } = false;
        public int Interval { get; set; } = 5000;
        public string Template { get; set; } = "{0}/{1} online. {2} TPS";
    }
}
