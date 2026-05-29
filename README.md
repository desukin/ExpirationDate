# Expiration Date

A mod for **Slay the Spire 2** that adds a set of challenging and flavorful run modifiers, configurable via the in-game Mod Configuration menu.

Author: **minimento**  
Version: **1.0.0**  
Dependencies: **BaseLib**, **HarmonyLib**

---

## Features

| Toggle | Effect |
|--------|--------|
| 🕒 **Expiration Enabled** | All cards obtained during the run are removed from your deck after **5 combats**. Plan your deck carefully — nothing lasts forever. |
| ✨ **Random Enchant Enabled** | Each card that enters your deck receives a **random vanilla enchantment**. Adds unpredictability to every run. |
| 📈 **Scaling HP Enabled** | Enemy HP scales with **real-time play duration**: +1% Max HP per minute. The longer you play, the tougher the fights. |
| ❓ **Hide Enemy HP Enabled** | Enemy HP and block values are hidden (shown as "?"). The health bar itself remains visible for a rough estimate. |

All features are independently togglable in the mod config menu.

---

## Installation

1. Install [BaseLib](https://github.com/minimento/BaseLib) (v3.1.3+)
2. Download the latest release of Expiration Date
3. Extract into `Slay the Spire 2/mods/ExpirationDate/`
4. Enable the mod in the in-game mod manager

### Build from Source

```bash
cd ExpirationDate_backup
dotnet build CustomModifiers.csproj -c Release
```

The built `.dll` and `.json` will be automatically copied to your mods folder.

---

## Project Structure

```
ExpirationDate/
├── ExpirationDate.json          # Mod manifest
├── CustomModifiers.csproj       # .NET 9.0 project file
├── ModEntry.cs                  # Entry point & Harmony patching
├── ExpirationDateConfig.cs      # Configuration UI
├── AllRemorsefulMod.cs          # Core logic (4 Harmony patches)
├── RandomEnchantment.cs         # Random enchantment logic
├── localization/
│   └── settings_ui.json         # UI localization strings
├── project.godot                # Godot project (for PCK export)
└── export_presets.cfg           # Godot export config
```

---

## Compatibility

- **Game version**: v0.103.2+
- **BaseLib**: v3.1.3+
- **affects_gameplay**: `true` — all players in multiplayer must have this mod installed
