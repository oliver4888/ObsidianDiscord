using ObsidianDiscord.Configuration.Attributes;

namespace ObsidianDiscord.Configuration.Models
{
    [ConfigFileName("Server-0\\config.json")]
    public class ObsidianConfig
    {
        // MaxPlayers is the only value I care about from the config.json file.
        public int MaxPlayers { get; set; }
    }
}
