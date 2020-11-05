using Newtonsoft.Json;

namespace ObsidianDiscord
{
    internal static class ConfigLoader
    {
        const string _configFile = "ObsidianDiscord.json";

        static DiscordPlugin Plugin => DiscordPlugin.Instance;

        internal static Config LoadConfig()
        {
            if (!Plugin.IFileReader.FileExists(_configFile))
            {
                Plugin.Logger.LogWarning($"Config file §e{_configFile}§r doesn't exist. Creating a new one.");

                Config config = new Config();
                Plugin.IFileWriter.WriteAllText(_configFile, JsonConvert.SerializeObject(config, Formatting.Indented));

                return config;
            }

            string json = Plugin.IFileReader.ReadAllText(_configFile);

            return JsonConvert.DeserializeObject<Config>(json);
        }
    }
}
