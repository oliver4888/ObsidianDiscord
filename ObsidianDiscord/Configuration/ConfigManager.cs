using System;
using System.Reflection;
using System.Threading.Tasks;

using Newtonsoft.Json;

using ObsidianDiscord.Configuration.Attributes;

namespace ObsidianDiscord.Configuration
{
    internal class ConfigManager : PluginRef
    {
        internal async Task<T> LoadConfig<T>(bool createMissing = true)
        {
            string fileName = GetFileName<T>();
            if (fileName == null)
                throw new Exception($"{nameof(LoadConfig)} was called for type {typeof(T).FullName}, but no {nameof(ConfigFileNameAttribute)} was found on the type.");

            return await LoadConfig<T>(fileName, createMissing);
        }

        internal async Task<T> LoadConfig<T>(string fileName, bool createMissing = true)
        {
            if (!IFileReader.FileExists(fileName))
            {
                if (!createMissing)
                    return default;

                Logger.LogWarning($"Config file §e{fileName}§r doesn't exist. Creating a new one.");

                T defaultConfig = Activator.CreateInstance<T>();

                await SaveConfig(fileName, defaultConfig);

                return defaultConfig;
            }

            string json = await IFileReader.ReadAllTextAsync(fileName);

            return JsonConvert.DeserializeObject<T>(json);
        }

        internal async Task SaveConfig<T>(T config) => await SaveConfig(GetFileName<T>(), config);

        private async Task SaveConfig<T>(string fileName, T config) =>
            await IFileWriter.WriteAllTextAsync(fileName, JsonConvert.SerializeObject(config, Formatting.Indented));

        private string GetFileName<T>() => typeof(T).GetCustomAttribute<ConfigFileNameAttribute>()?.FileName ?? null;
    }
}
