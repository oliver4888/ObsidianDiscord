using Newtonsoft.Json;

namespace ObsidianDiscord
{
    internal class ConfigLoader : PluginRef
    {
        const string _configFile = "ObsidianDiscord.json";

        internal Config LoadConfig()
        {
            if (!IFileReader.FileExists(_configFile))
            {
                Logger.LogWarning($"Config file §e{_configFile}§r doesn't exist. Creating a new one.");

                Config config = new Config();
                IFileWriter.WriteAllText(_configFile, JsonConvert.SerializeObject(config, Formatting.Indented));

                return config;
            }

            string json = IFileReader.ReadAllText(_configFile);

            return JsonConvert.DeserializeObject<Config>(json);
        }
    }
}
