# ObsidianDiscord
[![Build status](https://ci.appveyor.com/api/projects/status/de0ldta7wu380kk3/branch/master?svg=true)](https://ci.appveyor.com/project/oliver4888/obsidiandiscord/branch/master)

A [Discord](https://discord.com) chat bridge for Minecraft [Obsidian](https://github.com/Naamloos/Obsidian) servers.

## Roadmap
_Items may be added or removed at any time_
- [x] Player join / leave messages
  - [x] Custom message formatting
- [x] Chat sync
  - [x] Discord => Minecraft
  - [x] Minecraft => Discord
  - [ ] Message filtering & manipulation
    - [ ] Remove Minecraft colour codes from messages
    - [ ] Granular permissions to allow / disallow @everyone, @here and `<@id>` mentions
    - [ ] Replace `<@id>` mentions with the users name
- [ ] Account linking
  - [ ] Allow players to link their Discord accounts to a Minecraft account
  - [ ] Use Minecraft names when sending messages from Discord
  - [ ] Use Discord webhooks to send Minecraft => Discord messages with the authors Discord pfp and username
    - [x] Fallback to their Minecraft avatar face when the account isn't linked
- [x] Bot status
  - [x] Player count and TPS in the bot's status
  - [x] Customizable message format
- [ ] Wiki
  - [ ] Setup guide
  - [ ] Configuration options documentation
  - [ ] Required bot permissions
