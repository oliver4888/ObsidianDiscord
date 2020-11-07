using Obsidian.API.Plugins.Services;

namespace ObsidianDiscord
{
    internal class PluginRef
    {
        // Reference
        internal DiscordPlugin Plugin => DiscordPlugin.Instance;

        // Injected Utilities
        internal ILogger Logger => Plugin.Logger;
        internal IFileReader IFileReader => Plugin.IFileReader;
        internal IFileWriter IFileWriter => Plugin.IFileWriter;
    }
}
