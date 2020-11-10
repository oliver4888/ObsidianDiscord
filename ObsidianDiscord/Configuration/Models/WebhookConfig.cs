using DSharpPlus.Entities;

using ObsidianDiscord.Configuration.Attributes;

namespace ObsidianDiscord.Configuration.Models
{
    [ConfigFileName("ObsidianDiscord_Webhook.json")]
    public class WebhookConfig
    {
        public ulong WebhookId { get; set; }

        internal DiscordWebhook Webhook;
    }
}
