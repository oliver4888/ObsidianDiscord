# ObsidianDiscord
[![Build status](https://ci.appveyor.com/api/projects/status/de0ldta7wu380kk3/branch/master?svg=true)](https://ci.appveyor.com/project/oliver4888/obsidiandiscord/branch/master)

A [Discord](https://discord.com) chat bridge for Minecraft [Obsidian](https://github.com/Naamloos/Obsidian) servers.

## Roadmap
_Items may be added or removed at any time_
- [x] Player join / leave messages
  - [x] Custom message formatting
- [ ] Chat sync
  - [x] Discord => MC
  - [x] MC => Discord
  - [ ] Message filtering (remove colour codes from Discord messages etc)
- [ ] Account linking
  - [ ] Allow players to link their Discord accounts to a MC account
  - [ ] Use MC names when sending messages from Discord
  - [ ] Use Discord webhooks to send MC => Discord messages with the authors Discord pfp and username
    - [x] Fallback to their MC avatar face when the account isn't linked
- [x] Bot status
  - [x] Player count and TPS in the bot's status
  - [x] Customizable message format
