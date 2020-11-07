using System;

using Newtonsoft.Json;

namespace ObsidianDiscord
{
    internal class ConfigLoader : PluginRef
    {
        internal T LoadConfig<T>(string fileName)
        {
            if (!IFileReader.FileExists(fileName))
            {
                Logger.LogWarning($"Config file §e{fileName}§r doesn't exist. Creating a new one.");

                T defaultConfig = Activator.CreateInstance<T>();

                IFileWriter.WriteAllText(fileName, JsonConvert.SerializeObject(defaultConfig, Formatting.Indented));
                return defaultConfig;
            }

            string json = IFileReader.ReadAllText(fileName);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
