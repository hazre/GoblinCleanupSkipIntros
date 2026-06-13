using System.Collections;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoblinCleanupSkipIntros.Patches;

[HarmonyPatch]
public static class BootPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Boot), nameof(Boot.Start))]
    static bool LoadMainMenuDirectlyPrefix(Boot __instance)
    {
        Plugin.Logger.LogInfo("LoadMainMenuDirectlyPrefix: Start of prefix");

        try
        {

            // 1. Hide splash canvases to avoid seeing the logo for even a frame
            // 2. Show CanvasLoading and its children to display a loading screen
            var activeScene = SceneManager.GetActiveScene();
            Plugin.Logger.LogInfo($"LoadMainMenuDirectlyPrefix: Active scene is {activeScene.name}");

            var rootObjects = activeScene.GetRootGameObjects();
            Plugin.Logger.LogInfo($"LoadMainMenuDirectlyPrefix: Found {rootObjects.Length} root game objects");

            foreach (var obj in rootObjects)
            {
                if (obj.name == "CanvasSplash" || obj.name == "CanvasDisclaimer")
                {
                    Plugin.Logger.LogInfo($"LoadMainMenuDirectlyPrefix: Setting {obj.name} to inactive");
                    obj.SetActive(false);
                }
                else if (obj.name == "CanvasLoading")
                {
                    Plugin.Logger.LogInfo($"LoadMainMenuDirectlyPrefix: Setting CanvasLoading to active");
                    obj.SetActive(true);

                    // Ensure CanvasGroup is fully visible
                    var canvasGroup = obj.GetComponent<CanvasGroup>();
                    if (canvasGroup != null)
                    {
                        Plugin.Logger.LogInfo("LoadMainMenuDirectlyPrefix: Setting CanvasLoading CanvasGroup alpha to 1");
                        canvasGroup.alpha = 1f;
                        canvasGroup.interactable = true;
                        canvasGroup.blocksRaycasts = true;
                    }
                }
            }



            // 3. Keep original Boot.Start behaviors
            Plugin.Logger.LogInfo("LoadMainMenuDirectlyPrefix: Performing PlayerPrefs cleanups");
            Cursor.visible = false;
            PlayerPrefs.DeleteKey("LastLobbyID");

            // 4. Set booted to true to bypass Boot.Update's original load sequence
            Plugin.Logger.LogInfo("LoadMainMenuDirectlyPrefix: Setting booted to true");
            __instance.booted = true;

            // 5. Start loading MainMenu asynchronously on the persistent Plugin instance
            Plugin.Logger.LogInfo("LoadMainMenuDirectlyPrefix: Starting LoadMainMenuAsync coroutine on Plugin.Instance");
            Plugin.Instance.StartCoroutine(LoadMainMenuAsync());
        }
        catch (System.Exception ex)
        {
            Plugin.Logger.LogError($"LoadMainMenuDirectlyPrefix: Exception occurred: {ex}");
        }

        Plugin.Logger.LogInfo("LoadMainMenuDirectlyPrefix: End of prefix, returning false");
        return false; // Prevent original Boot.Start from running
    }


    private static IEnumerator LoadMainMenuAsync()
    {
        Plugin.Logger.LogInfo("LoadMainMenuAsync: Coroutine started");
        yield return null;
        Plugin.Logger.LogInfo("LoadMainMenuAsync: After yield return null");

        Plugin.Logger.LogInfo("LoadMainMenuAsync: Starting SceneManager.LoadSceneAsync('MainMenu')");
        var asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
        if (asyncLoad == null)
        {
            Plugin.Logger.LogError("LoadMainMenuAsync: LoadSceneAsync('MainMenu') returned null!");
            yield break;
        }

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Plugin.Logger.LogInfo("LoadMainMenuAsync: Loading completed! Destroying CanvasLoading.");
        var canvasLoading = GameObject.Find("CanvasLoading");
        if (canvasLoading != null)
        {
            canvasLoading.SetActive(false);
            GameObject.Destroy(canvasLoading);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIHome), nameof(UIHome.Start))]
    static void SkipHomeScreen()
    {
        Plugin.Logger.LogInfo("SkipHomeScreen: Bypassing UIHome screen");
        UIHome.noMoreHome = true;
    }
}



