namespace ObsidianDiscord
{
    public class Config
    {
        public bool Enabled { get; set; } = false;
        public string Token { get; set; }
        public ulong GuildId { get; set; }

        public JoinLeaveMessageConfig JoinLeaveMessages { get; set; } = new JoinLeaveMessageConfig();
    }

    public class JoinLeaveMessageConfig
    {
        public bool Enabled { get; set; } = false;
        public ulong ChannelId { get; set; }
    }
}
