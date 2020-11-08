using System;

namespace ObsidianDiscord.Configuration.Attributes
{
    internal class ConfigFileNameAttribute : Attribute
    {
        internal string FileName { get; private set; }

        internal ConfigFileNameAttribute(string fileName) => FileName = fileName;
    }
}
