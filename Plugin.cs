using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using EFT;
using EFT.Hideout;
using EFT.UI;
using EFT.UI.Matchmaker;
using HarmonyLib;
using HarmonyLib.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;
using UIRefresh.Patches;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIRefresh
{
        [BepInPlugin("redlaser42.UI_Refresh", "redlaser42.UI_Refresh", "1.0.0")]
        public class Plugin : BaseUnityPlugin
        {


        public static ConfigEntry<bool>? EnableClockPatchConfig;
        public static ConfigEntry<bool>? ClockUsesSystemTimeConfig;
        public static ConfigEntry<bool>? HideQuickSlotsConfig;
        public static ConfigEntry<bool>? StanceSillhouetteConfig;
        public static ConfigEntry<bool>? PTTLocationNameConfig;
        public static ConfigEntry<bool>? HideBackgoundpPatch;
        public static ConfigEntry<bool>? DisableGroupConfig;
        public static ConfigEntry<bool>? HideOutMainMenuConfig;
        public static ConfigEntry<bool>? SkipPreRaidMenusConfig;
        public static ConfigEntry<bool>? MenuLayoutChangesConfig;
        public static ConfigEntry<bool>? ChangeUISceneOnLoading;
        public static ConfigEntry<bool>? HideMenuBackgroundInRaid;



        public static ConfigEntry<bool>? mapOnTaskBarConfig;
        public static ConfigEntry<string>? mapButtonTextConfig;



        public static GameObject locationMenuObj = null;



            public static ConfigEntry<Color> CustomsColorConfig { get; set; }
            public static ConfigEntry<Color> FactoryColorConfig { get; set; }
            public static ConfigEntry<Color> WoodsColorConfig { get; set; }
            public static ConfigEntry<Color> InterchangeColorConfig { get; set; }
            public static ConfigEntry<Color> ReserveColorConfig { get; set; }
            public static ConfigEntry<Color> ShorelineColorConfig { get; set; }
            public static ConfigEntry<Color> LighthouseColorConfig { get; set; }
            public static ConfigEntry<Color> GroundZeroColorConfig { get; set; }
            public static ConfigEntry<Color> StreetsColorConfig { get; set; }
            public static ConfigEntry<Color> LabsColorConfig { get; set; }


            public static bool initOnce = false;

            public static ConfigEntry<float>? CharacterZoom;


            private void Awake()
            {

            CustomsColorConfig = Config.Bind("Loading Screen Accent Colors","Customs", new Color(0.56f, 0.51f, 0.15f), ".");
            FactoryColorConfig = Config.Bind("Loading Screen Accent Colors", "Factory", new Color(1, .49f, .01f), ".");
            WoodsColorConfig = Config.Bind("Loading Screen Accent Colors", "Woods", new Color(0.01f, 0.35f, 0.07f), ".");
            InterchangeColorConfig = Config.Bind("Loading Screen Accent Colors", "Interchange", new Color(0.16f, 0.18f, 1), ".");
            ReserveColorConfig = Config.Bind("Loading Screen Accent Colors", "Reserve", new Color(0.58f, 0.08f, 0.03f), ".");
            ShorelineColorConfig = Config.Bind("Loading Screen Accent Colors", "Shoreline", new Color(1, 1, 0.54f), ".");
            LighthouseColorConfig = Config.Bind("Loading Screen Accent Colors", "Lighthouse", new Color (0.01f, 0.3f, 0.23f), ".");
            GroundZeroColorConfig = Config.Bind("Loading Screen Accent Colors", "Ground Zero", new Color(0.47f, 0.63f, 0.64f), ".");
            StreetsColorConfig = Config.Bind("Loading Screen Accent Colors", "Streets", new Color(0.61f, 1, 0.48f), ".");
            LabsColorConfig = Config.Bind("Loading Screen Accent Colors", "Labs", new Color(1, 1, 1), ".");


            DisableGroupConfig = Config.Bind(
            "1. Menu UI",                // Section name
            "c. Disable Group Widget",       // Setting key
            true,                     // Default value
            "Disables the Group buttons on the Task Bar."
            );

            ClockUsesSystemTimeConfig = Config.Bind(
            "1. Menu UI",                // Section name
            "e. Clock Uses System Time",       // Setting key
            false,                     // Default value
            "Have the clock widget use your system time."
            );

            MenuLayoutChangesConfig = Config.Bind(
            "1. Menu UI",                // Section name
            "a. Menu Layout Changes",       // Setting key
            true,                     // Default value
            "Enables the various edits to the layouts of menus."
            );
            if(MenuLayoutChangesConfig.Value)
            {
                new AreaScreenSubstrate_AwakePatch().Enable();
            }

            SkipPreRaidMenusConfig = Config.Bind(
            "1. Menu UI",                // Section name
            "b. Skip Pre-Raid Menus",       // Setting key
            true,                     // Default value
            "Skips Raid Settings and Insurance Menus."
            );

            EnableClockPatchConfig = Config.Bind(
                "1. Menu UI",                // Section name
                "d. Enable Clock Widget",       // Setting key
                true,                     // Default value
                "Enable or disable the raid clock widget."
            );
            if (EnableClockPatchConfig.Value)
            {
                new InventoryScreen_ShowPatch().Enable();
            }

            HideQuickSlotsConfig = Config.Bind(
                "2. HUD",                // Section name
                "a. Hide Quick Slots",       // Setting key
                true,                     // Default value
                "Hides the Quick Access slot UI in the HUD."
            );

            StanceSillhouetteConfig = Config.Bind(
            "2. HUD",                // Section name
            "b. Hide Stance Guy",       // Setting key
            true,                     // Default value
            "Hides the Stance Silhouette."
            );

            mapOnTaskBarConfig = Config.Bind(
                "1. Menu UI",                // Section name
                "f. Enable Map Button",       // Setting key
                true,                     // Default value
                "Enable or disable the Map button on the Taskbar."
            );
            if (mapOnTaskBarConfig.Value)
            {
                new MenuTaskBar_AwakePatch().Enable();
            }

            mapButtonTextConfig = Config.Bind(
            "1. Menu UI",                // Section name
            "c. Map button Text",       // Setting key
            "MAP",                     // Default value
            "The text that appears on the Map button"
            );




            ChangeUISceneOnLoading = Config.Bind(
            "1. Menu UI",                // Section name
            "g. Map Specific Loading Screen",       // Setting key
            true,                     // Default value
            "Changes the background scene when loading a raid per map."
            );

            HideMenuBackgroundInRaid = Config.Bind(
            "1. Menu UI",                // Section name
            "h. Hide Menu Background In Raid",       // Setting key
            true,                     // Default value
            "Hides the pause menu background when you are in a raid."
            );


            HideOutMainMenuConfig = Config.Bind(
            "z. Beta",                // Section name
            "a. Show Hideout in Main Menu",       // Setting key
            false,                     // Default value
            "Shows the Hideout in the main menu."
            );
            if (HideOutMainMenuConfig.Value)
            {
                new HideoutOverlay_ShowPatch().Enable();
            }

            PTTLocationNameConfig = Config.Bind(
                "z. Beta",                // Section name
                "b. Enable PTT Location Name",       // Setting key
                false,                     // Default value
                "Shows your Path To Tarkov out-of-raid location on the map."
            );
            if (PTTLocationNameConfig.Value)
            {
                new PTTLocationPatch().Enable();
            }

            new SideSelectionPatch().Enable();
            new LocationSelectionShowPatch().Enable();
            new RaidSettingsScreen_ShowPatch().Enable();
            new InsuranceScreenPatch().Enable();
            new AcceptLocationPatch().Enable();
            new TimeHasComeShowPatch().Enable();
            new FinalCountdownPatch().Enable();
            new MenuScreen_ShowPatch().Enable();
            new QuickSlotsHUD_ShowPatch().Enable();
            new SessionResultExitStatus_ShowPatch().Enable();


            }

        public static ConfigEntry<Color> GetMapColorConfig(string mapName)
        {
            switch (mapName)
            {
                case "Factory":
                    return Plugin.FactoryColorConfig;
                case "Customs":
                    return Plugin.CustomsColorConfig;
                case "Woods":
                    return Plugin.WoodsColorConfig;
                case "Interchange":
                    return Plugin.InterchangeColorConfig;
                case "Reserve":
                    return Plugin.ReserveColorConfig;
                case "Shoreline":
                    return Plugin.ShorelineColorConfig;
                case "Lighthouse":
                    return Plugin.LighthouseColorConfig;
                case "Ground Zero":
                    return Plugin.GroundZeroColorConfig;
                case "Streets of Tarkov":
                    return Plugin.StreetsColorConfig;
                case "Labs":
                    return Plugin.LabsColorConfig;
                default:
                    return LabsColorConfig; // fallback if map name not recognized
            }
        }

        public static void setLoadRaidBackground (string mapName)
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
                    environmentUI.SetEnvironmentAsync(EEnvironmentUIType.LaboratoryEnvironmentUiType);
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
                    environmentUI.SetEnvironmentAsync(EEnvironmentUIType.TheUnheardEditionEnvironmentUiType);
                    return;
                case "Labs":
                    environmentUI.SetEnvironmentAsync(EEnvironmentUIType.LaboratoryEnvironmentUiType);
                    return;
                default:
                    environmentUI.SetEnvironmentAsync(EEnvironmentUIType.FactoryEnvironmentUiType);
                    return;
            }
        }



        public static GameObject FindRootObject(string sceneName, string objectName)
        {
            // Get the scene by name (you could also use SceneManager.GetSceneAt(index))
            Scene scene = SceneManager.GetSceneByName(sceneName);

            if (!scene.isLoaded)
            {
                Debug.LogError($"Scene {sceneName} is not loaded!");
                return null;
            }

            // Get all root objects in the scene
            GameObject[] rootObjects = scene.GetRootGameObjects();

            foreach (var go in rootObjects)
            {
                if (go.name == objectName)
                {
                    return go;
                }
            }

            return null; // Not found
        }

        public static void focusHideoutArea(HideoutScreenOverlay instance, int area)
        {
        
            var areaToFocus = GameObject.Find("Common UI/Common UI/HideoutScreenRear/HideoutScreenOverlay/BottomAreasPanel/Scroll View/Viewport/Content/").transform.GetChild(area).GetComponent<AreaPanel>();
            if (areaToFocus != null)
            {
                instance.method_13(areaToFocus);
                return;
            }
        
        }

        public static void callTarkovMenu(EMenuType menu)
        {
            TarkovApplication.Exist(out TarkovApplication tarkovApp);
            if (tarkovApp != null)
            {
                var menuOperation = AccessTools.Field(tarkovApp.GetType(), "_menuOperation").GetValue(tarkovApp) as MainMenuControllerClass;
                if (menuOperation != null)
                {
                    menuOperation.ShowScreen(menu, true);
                    return;
                }
            }
        }

        public static void findLocationAcceptMenu() 
        {
            Plugin.locationMenuObj = GameObject.Find("Menu UI/UI/Location Selection Screen/");
        }


        public static class RandomHelper
        {
            private static readonly System.Random rng = new System.Random();

            // Pass in any number of ints, returns one at random
            public static int PickRandom(params int[] values)
            {
                if (values == null || values.Length == 0)
                    throw new ArgumentException("Must provide at least one value.", nameof(values));

                int index = rng.Next(values.Length); // random index
                return values[index];
            }
        }

        public class MenuWatcher : MonoBehaviour
        {
            public System.Action? OnMenuDisabled;

            private void OnDisable()
            {
                // Called when this GameObject is deactivated
                OnMenuDisabled?.Invoke();
            }
        }


        public static GameObject FindFPSCam()
        {
            GameObject FPSCamera = Plugin.FindRootObject("CommonUIScene", "FPS Camera");
            if (FPSCamera == null)
            {
                FPSCamera = Plugin.FindRootObject("MenuUIScene", "FPS Camera");
            }
            if (FPSCamera == null)
            {
                FPSCamera = Plugin.FindRootObject("DontDestroyOnLoad", "FPS Camera");
            }
            if (FPSCamera == null)
            {
                return null;
            }
            return FPSCamera;
        }


        public static string GetRaidTime(ISession ___iSession)
        {
            if (Plugin.ClockUsesSystemTimeConfig.Value)
            {
                return DateTime.Now.ToString("HH:mm:ss");

            }
            else
            {
                // If IDNC is installed, get raid time from helper.
                if (Chainloader.PluginInfos.ContainsKey("Jehree.ImmersiveDaylightCycle"))
                {
                    return Plugin.TryGetImmersiveTime();
                }

                // Otherwise fallback to default time
                return ___iSession.GetCurrentLocationTime.ToString("HH:mm:ss");
            }
            }

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
}
