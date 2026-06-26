<table>
  <tr>
    <td><img src="https://raw.githubusercontent.com/hazre/GoblinCleanupSkipIntros/refs/heads/main/icon.png" width="96" alt="More Players icon"></td>
    <td>
      <h1>Skip Intros</h1>
      <p>A <a href="https://store.steampowered.com/app/2748340/Goblin_Cleanup/">Goblin Cleanup</a> mod that skips intro videos and splash screens on startup.</p>
    </td>
  </tr>
</table>

[![Thunderstore Badge](https://modding.resonite.net/assets/available-on-thunderstore.svg)](https://thunderstore.io/c/goblin-cleanup/)


## Installation (Manual)
1. Install [BepInExPack for Goblin Cleanup](https://github.com/hazre/BepInExPack-GoblinCleanup) (includes BepInEx + unstripped corlibs).
2. Download the latest release ZIP from the [Releases](https://github.com/hazre/GoblinCleanupSkipIntros/releases) page.
3. Extract the ZIP and copy `GoblinCleanupSkipIntros.dll` to your BepInEx plugins folder:
   - **Default location:** `C:\Program Files (x86)\Steam\steamapps\common\Goblin Cleanup\BepInEx\plugins\`
4. Start the game.

## Development
 
Install [mise](https://mise.jdx.dev/getting-started.html) and run `mise install` to set up tools.
 
```bash
mise run build    # Build the DLL
mise run package  # Build the Thunderstore package
```
 
Run `mise tasks` to list all available tasks.


## License

This project is licensed under MIT License. See [LICENSE](LICENSE) for details.
