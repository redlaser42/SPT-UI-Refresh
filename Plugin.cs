using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using EFT;
using EFT.Hideout;
using EFT.UI;
using HarmonyLib;
using System;
using UIRefresh.Patches;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIRefresh
{
        [BepInPlugin("redlaser42.UI_Refresh", "redlaser42.UI_Refresh", "2.0.0")]
        public class Plugin : BaseUnityPlugin
        {

        public static ConfigEntry<bool>? EnableClockPatchConfig;
        public static ConfigEntry<bool>? ClockUsesSystemTimeConfig;
        public static ConfigEntry<bool>? HideQuickSlotsConfig;
        public static ConfigEntry<bool>? StanceSillhouetteConfig;
        public static ConfigEntry<bool>? HideBackgoundpPatch;
        public static ConfigEntry<bool>? DisableGroupConfig;
        public static ConfigEntry<bool>? HideOutMainMenuConfig;
        public static ConfigEntry<bool>? SkipPreRaidMenusConfig;
        public static ConfigEntry<bool>? MenuLayoutChangesConfig;
        public static ConfigEntry<bool>? ChangeUISceneOnLoading;
        public static ConfigEntry<bool>? HideMenuBackgroundInRaid;
        public static ConfigEntry<bool>? mapOnTaskBarConfig;

        public static bool initOnce = false;

        public static ConfigEntry<string>? mapButtonTextConfig;

        public static GameObject locationMenuObj = null;

        public static ConfigEntry<float>? CharacterZoom;

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

            private void Awake()
            {
            //Bind Color Configs
            CustomsColorConfig = Config.Bind("Loading Screen Accent Colors","Customs", new Color(0.55f, 0.55f, 0.08f), ".");
            FactoryColorConfig = Config.Bind("Loading Screen Accent Colors", "Factory", new Color(0.4f, .12f, .10f), ".");
            WoodsColorConfig = Config.Bind("Loading Screen Accent Colors", "Woods", new Color(0.01f, 0.36f, 0.16f), ".");
            InterchangeColorConfig = Config.Bind("Loading Screen Accent Colors", "Interchange", new Color(0.01f, 0.34f, 1), ".");
            ReserveColorConfig = Config.Bind("Loading Screen Accent Colors", "Reserve", new Color(0.49f, 0.06f, 0.01f), ".");
            ShorelineColorConfig = Config.Bind("Loading Screen Accent Colors", "Shoreline", new Color(0.43f, 0.19f, 0.43f), ".");
            LighthouseColorConfig = Config.Bind("Loading Screen Accent Colors", "Lighthouse", new Color (0.90f, 0.6f, 0.13f), ".");
            GroundZeroColorConfig = Config.Bind("Loading Screen Accent Colors", "Ground Zero", new Color(0.53f, 0.6f, 0.67f), ".");
            StreetsColorConfig = Config.Bind("Loading Screen Accent Colors", "Streets", new Color(0.55f, .5f, 0.5f), ".");
            LabsColorConfig = Config.Bind("Loading Screen Accent Colors", "Labs", new Color(1, 1, 1), ".");

            //Bind Bool Configs
            DisableGroupConfig = Config.Bind("1. Menu UI","c. Disable Group Widget",true,"Disables the Group buttons on the Task Bar.");
            ClockUsesSystemTimeConfig = Config.Bind("1. Menu UI","e. Clock Uses System Time", false,"Have the clock widget use your system time.");
            MenuLayoutChangesConfig = Config.Bind("1. Menu UI", "a. Menu Layout Changes", true, "Enables the various edits to the layouts of menus.");
            if(MenuLayoutChangesConfig.Value)
            {
                new AreaScreenSubstrate_AwakePatch().Enable();
            }

            SkipPreRaidMenusConfig = Config.Bind("1. Menu UI", "b. Skip Pre-Raid Menus", true, "Skips Raid Settings and Insurance Menus.");
            EnableClockPatchConfig = Config.Bind("1. Menu UI", "d. Enable Clock Widget", true, "Enable or disable the raid clock widget.");
            if (EnableClockPatchConfig.Value)
            {
                new InventoryScreen_ShowPatch().Enable();
            }

            HideQuickSlotsConfig = Config.Bind("2. HUD", "a. Hide Quick Slots", false, "Hides the Quick Access slot UI in the HUD.");
            StanceSillhouetteConfig = Config.Bind("2. HUD", "b. Hide Stance Guy", false, "Hides the Stance Silhouette.");
            mapOnTaskBarConfig = Config.Bind("1. Menu UI", "f. Enable Map Button", true, "Enable or disable the Map button on the Taskbar.");
            if (mapOnTaskBarConfig.Value)
            {
                new MenuTaskBar_AwakePatch().Enable();
            }

            mapButtonTextConfig = Config.Bind("1. Menu UI", "c. Map button Text", "MAP", "The text that appears on the Map button");
            ChangeUISceneOnLoading = Config.Bind("1. Menu UI", "g. Map Specific Loading Screen", true, "Changes the background scene when loading a raid per map.");
            HideMenuBackgroundInRaid = Config.Bind("1. Menu UI", "h. Hide Menu Background In Raid", true, "Hides the pause menu background when you are in a raid.");
            HideOutMainMenuConfig = Config.Bind("z. Beta", "a. Show Hideout in Main Menu", false, "Shows the Hideout in the main menu.");
            if (HideOutMainMenuConfig.Value)
            {
                new HideoutOverlay_ShowPatch().Enable();
            }

            //Checks if you're doing it right. 
            Utils.checkFika();
            if (Utils.playingFika)
            {
                //new FIKA_OnlinePlayers_Patch().Enable();
            }

            new SideSelection_ShowPatch().Enable();
            new LocationSelection_ShowPatch().Enable();
            new RaidSettingsScreen_ShowPatch().Enable();
            new InsuranceScreen_ShowPatch().Enable();
            // new MatchMakerAcceptScreen_Patch().Enable();
            new TimeHasCome_ShowPatch().Enable();
            new FinalCountdown_ShowPatch().Enable();
            new MenuScreen_ShowPatch().Enable();
            new HideTopGlowPatch().Enable();
            new HideAlphaWarning_Patch().Enable();
            new HideGameModePatch().Enable();
            new HideVersionLabelPatch().Enable();


            //new QuickSlotsHUD_ShowPatch().Enable();
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
    }   
}
