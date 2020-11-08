using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ObsidianDiscord.Configuration
{
    internal class ConfigLoader : PluginRef
    {
        internal async Task<T> LoadConfig<T>(string fileName, bool createMissing = true)
        {
            if (!IFileReader.FileExists(fileName))
            {
                if (!createMissing)
                    return default;

                Logger.LogWarning($"Config file §e{fileName}§r doesn't exist. Creating a new one.");

                T defaultConfig = Activator.CreateInstance<T>();

                await IFileWriter.WriteAllTextAsync(fileName, JsonConvert.SerializeObject(defaultConfig, Formatting.Indented));
                return defaultConfig;
            }

            string json = await IFileReader.ReadAllTextAsync(fileName);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
