namespace ObsidianDiscord.Configuration.Models
{
    public class Config
    {
        public bool Enabled { get; set; } = false;
        public string Token { get; set; }
        public ulong GuildId { get; set; }

        public JoinLeaveMessageConfig JoinLeaveMessages { get; set; } = new JoinLeaveMessageConfig();
        public ChatSyncConfig ChatSync { get; set; } = new ChatSyncConfig();
    }

    public class JoinLeaveMessageConfig
    {
        public bool Enabled { get; set; } = false;
        public ulong ChannelId { get; set; }
    }

    public class ChatSyncConfig
    {
        public bool Enabled { get; set; } = false;
        public ulong ChannelId { get; set; }
    }
}
