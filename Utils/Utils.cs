using BepInEx;
using BepInEx.Bootstrap;
using EFT.Hideout;
using EFT.UI;
using HarmonyLib;
using System;
using UIRefresh;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils
{
    public static bool playingFika;
    public static void checkFika() {
        if (Chainloader.PluginInfos.ContainsKey("com.fika.core"))
        {
            playingFika = true;
            return;
        }
        playingFika = false;
        return;
    }


    //Find objects in a scene that have no parent. 
    public static GameObject FindRootObject(string sceneName, string objectName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.isLoaded)
        {
            Debug.LogError($"Scene {sceneName} is not loaded!");
            return null;
        }

        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (var go in rootObjects)
        {
            if (go.name == objectName)
            {
                return go;
            }
        }
        return null;
    }

    //Auto selects an hideout area when hideout loads.
    public static void focusHideoutArea(HideoutScreenOverlay instance, int area)
    {
        var areaToFocus = GameObject.Find("Common UI/Common UI/HideoutScreenRear/HideoutScreenOverlay/BottomAreasPanel/Scroll View/Viewport/Content/").transform.GetChild(area).GetComponent<AreaPanel>();
        if (areaToFocus != null)
        {
            instance.method_13(areaToFocus);
            return;
        }
    }

    //Object watcher component attached to map screen. Un-highights map button on taskbar when map is disabled.
    public class MenuWatcher : MonoBehaviour
    {
        public System.Action? OnMenuDisabled;
        private void OnDisable()
        {
            OnMenuDisabled?.Invoke();
        }
    }

    public static GameObject FindFPSCam()
    {
        GameObject FPSCamera = FindRootObject("CommonUIScene", "FPS Camera");
        if (FPSCamera == null)
        {
            FPSCamera = FindRootObject("MenuUIScene", "FPS Camera");
        }
        if (FPSCamera == null)
        {
            FPSCamera = FindRootObject("DontDestroyOnLoad", "FPS Camera");
        }
        if (FPSCamera == null)
        {
            return null;
        }
        return FPSCamera;
    }

    //Get raid time for clock.
    public static string GetRaidTime(ISession ___iSession)
    {
        // Does clock use system time?
        if (Plugin.ClockUsesSystemTimeConfig.Value)
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
        else
        {
            // If Immersive Day Night Cycle is installed, get raid time from helper.
            if (Chainloader.PluginInfos.ContainsKey("Jehree.ImmersiveDaylightCycle"))
            {
                return TryGetImmersiveTime();
            }
            // Otherwise default  raid time
            return ___iSession.GetCurrentLocationTime.ToString("HH:mm:ss");
        }
    }

    //Attempts to get Immersive Day Night Cycle's time. 
    public static string TryGetImmersiveTime()
    {
        var type = AccessTools.TypeByName("Jehree.ImmersiveDaylightCycle.Helpers.Utils");
        if (type != null)
        {
            var method = AccessTools.Method(type, "GetCurrentTime");

            var result = method.Invoke(null, null);
            if (result is DateTime dt)
            {
                return dt.ToString("HH:mm:ss");
            }
        }
        return "??:??";
    }

    public static void ShowEnvironmentUI(bool active)
    {
        EnvironmentUI environmentUI = MonoBehaviourSingleton<EnvironmentUI>.Instance;
        environmentUI.ShowEnvironment(active);
    }

    public static void ToggleEnvironmentBackground(bool active)
    {
        EnvironmentUI environmentUI = MonoBehaviourSingleton<EnvironmentUI>.Instance;
        environmentUI.transform.GetChild(2).GetChild(0).gameObject.SetActive(active);
    }
}