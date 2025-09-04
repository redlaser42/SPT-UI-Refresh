using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using UIRefresh.Patches;
using UnityEngine;

namespace UIRefresh
{
        [BepInPlugin("redlaser42.UI_Refresh", "redlaser42.UI_Refresh", "1.0.0")]
        public class Plugin : BaseUnityPlugin
        {

        public static ManualLogSource? LogSource;

        public static ConfigEntry<bool>? EnableClockPatchConfig;
        public static ConfigEntry<bool>? EnableQuickSlotsConfig;
        public static ConfigEntry<bool>? EnableHideStanceConfig;
        public static ConfigEntry<bool>? EnableMapConfig;
        public static ConfigEntry<bool>? PTTLocationNameConfig;
        public static ConfigEntry<bool>? HideBackgoundpPatch;

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




        public static ConfigEntry<float>? CharacterZoom;


        private void Awake()
            {
            new SideSelectionPatch().Enable();
            new LocationSelectionPatch().Enable();
            new RaidSettingsScreenPatch().Enable();
            new InsuranceScreenPatch().Enable();
            new AcceptLocationPatch().Enable();
            new TimeHasComeShowPatch().Enable();
            new FinalCountdownPatch().Enable();




            CustomsColorConfig = Config.Bind("Colors","Customs Color", new Color(0.56f, 0.51f, 0.15f), ".");
            FactoryColorConfig = Config.Bind("Colors", "Factory Color", new Color(1, .49f, .01f), ".");
            WoodsColorConfig = Config.Bind("Colors", "Woods Color", new Color(0.01f, 0.35f, 0.07f), ".");
            InterchangeColorConfig = Config.Bind("Colors", "Interchange Color", new Color(0.16f, 0.18f, 1), ".");
            ReserveColorConfig = Config.Bind("Colors", "Reserve Color", new Color(0.58f, 0.08f, 0.03f), ".");
            ShorelineColorConfig = Config.Bind("Colors", "Shoreline Color", new Color(1, 1, 0.54f), ".");
            LighthouseColorConfig = Config.Bind("Colors", "Lighthouse Color", new Color (0.01f, 0.3f, 0.23f), ".");
            GroundZeroColorConfig = Config.Bind("Colors", "Ground Zero Color", new Color(0.47f, 0.63f, 0.64f), ".");
            StreetsColorConfig = Config.Bind("Colors", "Streets Color", new Color(0.61f, 1, 0.48f), ".");
            LabsColorConfig = Config.Bind("Colors", "Labs Color", new Color(1, 1, 1), ".");






            EnableClockPatchConfig = Config.Bind(
                "General",                // Section name
                "Enable Clock Widget",       // Setting key
                true,                     // Default value
                "Enable or disable the raid clock widget."
            );
            if (EnableClockPatchConfig.Value)
            {
                new ClockPatch().Enable();
            }

            EnableQuickSlotsConfig = Config.Bind(
                "HUD",                // Section name
                "Hide Quick Slots",       // Setting key
                true,                     // Default value
                "Hides the Quick Access slot UI in the HUD."
            );
            if (EnableQuickSlotsConfig.Value)
            {
                new QuickSlotsHUDPatch().Enable();
            }

            EnableHideStanceConfig = Config.Bind(
            "HUD",                // Section name
            "Hide Stance Guy",       // Setting key
            false,                     // Default value
            "Hides the Stance Silhouette."
);
            if (EnableHideStanceConfig.Value)
            {
                new StanceSilhouettePatch().Enable();
            }

            EnableMapConfig = Config.Bind(
                "General",                // Section name
                "Enable Map Button",       // Setting key
                true,                     // Default value
                "Enable or disable the Map button on the Taskbar."
            );
            if (EnableMapConfig.Value)
            {
                new MapOnTaskBarPatch().Enable();
            }


            PTTLocationNameConfig = Config.Bind(
                "Mods",                // Section name
                "Enable PTT Location Name",       // Setting key
                false,                     // Default value
                "Shows your Path To Tarkov out-of-raid location on the map."
            );
            if (PTTLocationNameConfig.Value)
            {
                new PTTLocationPatch().Enable();
            }
            }


        public static Color GetMapColor(string mapName)
        {
            switch (mapName)
            {
                case "Factory":
                    return Plugin.FactoryColorConfig.Value;
                case "Customs":
                    return Plugin.CustomsColorConfig.Value;
                case "Woods":
                    return Plugin.WoodsColorConfig.Value;
                case "Interchange":
                    return Plugin.InterchangeColorConfig.Value;
                case "Reserve":
                    return Plugin.ReserveColorConfig.Value;
                case "Shoreline":
                    return Plugin.ShorelineColorConfig.Value;
                case "Lighthouse":
                    return Plugin.LighthouseColorConfig.Value;
                case "Ground Zero":
                    return Plugin.GroundZeroColorConfig.Value;
                case "Streets of Tarkov":
                    return Plugin.StreetsColorConfig.Value;
                case "Labs":
                    return Plugin.LabsColorConfig.Value;
                default:
                    return Color.white; // fallback if map name not recognized
            }
        }
    }
    }
