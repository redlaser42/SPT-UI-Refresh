using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using EFT;
using EFT.Hideout;
using EFT.UI;
using HarmonyLib;
using System;
using UIRefresh;
using UIRefresh.Config;

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
    public static GameObject? FindRootObject(string sceneName, string objectName)
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

    public static GameObject? FindFPSCam()
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
        if (Plugin.Instance.UIRefreshConfig.ClockUsesSystemTimeConfig.Value)
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
        else
        {
            // If Immersive Day Night Cycle is installed, get raid time from helper.
            if (Chainloader.PluginInfos.ContainsKey("Jehree.ImmersiveDaylightCycle"))
            {
                return ___iSession.GetCurrentLocationTime.ToString("HH:mm:ss");

                //return TryGetImmersiveTime();
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

    public static void setLoadRaidBackground(string mapName)
    {
        EnvironmentUI environmentUI = MonoBehaviourSingleton<EnvironmentUI>.Instance;

        switch (mapName)
        {
            case "Factory":
                environmentUI.SetEnvironmentAsync(EEnvironmentUIType.FactoryEnvironmentUiType);
                return;
            case "Customs":
                environmentUI.SetEnvironmentAsync(EEnvironmentUIType.FactoryEnvironmentUiType);
                return;
            case "Woods":
                environmentUI.SetEnvironmentAsync(EEnvironmentUIType.WoodEnvironmentUiType);
                return;
            case "Interchange":
                environmentUI.SetEnvironmentAsync(EEnvironmentUIType.TheUnheardEditionEnvironmentUiType);
                return;
            case "Reserve":
                environmentUI.SetEnvironmentAsync(EEnvironmentUIType.FactoryEnvironmentUiType);
                return;
            case "Shoreline":
                environmentUI.SetEnvironmentAsync(EEnvironmentUIType.WoodEnvironmentUiType);
                return;
            case "Lighthouse":
                environmentUI.SetEnvironmentAsync(EEnvironmentUIType.WoodEnvironmentUiType);
                return;
            case "Ground Zero":
                environmentUI.SetEnvironmentAsync(EEnvironmentUIType.TheUnheardEditionEnvironmentUiType);
                return;
            case "Streets of Tarkov":
                environmentUI.SetEnvironmentAsync(EEnvironmentUIType.LaboratoryEnvironmentUiType);
                return;
            case "Labs":
                environmentUI.SetEnvironmentAsync(EEnvironmentUIType.LaboratoryEnvironmentUiType);
                return;
            default:
                environmentUI.SetEnvironmentAsync(EEnvironmentUIType.FactoryEnvironmentUiType);
                return;
        }
    }
    public static ConfigEntry<Color> GetMapColorConfig(string mapName)
    {
        switch (mapName)
        {
            case "Factory":
                return Plugin.Instance.UIRefreshConfig.FactoryColorConfig;
            case "Customs":
                return Plugin.Instance.UIRefreshConfig.CustomsColorConfig;
            case "Woods":
                return Plugin.Instance.UIRefreshConfig.WoodsColorConfig;
            case "Interchange":
                return Plugin.Instance.UIRefreshConfig.InterchangeColorConfig;
            case "Reserve":
                return Plugin.Instance.UIRefreshConfig.ReserveColorConfig;
            case "Shoreline":
                return Plugin.Instance.UIRefreshConfig.ShorelineColorConfig;
            case "Lighthouse":
                return Plugin.Instance.UIRefreshConfig.LighthouseColorConfig;
            case "Ground Zero":
                return Plugin.Instance.UIRefreshConfig.GroundZeroColorConfig;
            case "Streets of Tarkov":
                return Plugin.Instance.UIRefreshConfig.StreetsColorConfig;
            case "Labs":
                return Plugin.Instance.UIRefreshConfig.LabsColorConfig;
            default:
                return Plugin.Instance.UIRefreshConfig.LabsColorConfig; // fallback if map name not recognized
        }
    }
}