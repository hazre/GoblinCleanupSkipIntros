using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace GoblinCleanupSkipIntros;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    internal static Plugin Instance = null!;
    internal static new ManualLogSource Logger = null!;
    internal static Harmony HarmonyInstance = null!;

    private void Awake()
    {
        Instance = this;
        Logger = base.Logger;

        HarmonyInstance = new Harmony(Id);

        Logger.LogInfo($"Plugin {Id} loaded!");

        HarmonyInstance.PatchAll(typeof(Plugin).Assembly);

        Logger.LogInfo("All patches applied");
    }

    void OnDestroy()
    {
        HarmonyInstance?.UnpatchSelf();
        Logger.LogInfo("Plugin unloaded");
    }
}
