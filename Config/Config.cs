using BepInEx.Configuration;
using System;
using UIRefresh.Patches;
using UnityEngine;

namespace UIRefresh.Config
{
    public class UIRefreshConfig
    {
        public ConfigEntry<bool> EnableClockPatchConfig { get; set; }
        public ConfigEntry<bool> ClockUsesSystemTimeConfig { get; set; }
        public ConfigEntry<bool> HideStanceSillhouette { get; set; }
        public ConfigEntry<bool> HideGesturesQuickPanel { get; set; }
        public ConfigEntry<bool> HideRaidTimerWarning { get; set; }
        public ConfigEntry<bool> HideAmmoPanel { get; set; }

        public ConfigEntry<bool> HideBackpackInventory { get; set; }



        public ConfigEntry<bool> MoveHealthPanelConfig { get; set; }
        public ConfigEntry<bool> HideOutMainMenuConfig { get; set; }
        public ConfigEntry<bool> SkipPreRaidMenusConfig { get; set; }
        public ConfigEntry<bool> MenuLayoutChangesConfig { get; set; }
        public ConfigEntry<bool> ChangeUISceneOnLoading { get; set; }
        public ConfigEntry<bool> HideMenuBackgroundInRaid { get; set; }
        public ConfigEntry<bool> mapOnTaskBarConfig { get; set; }
        public ConfigEntry<bool> DisableGroupConfig { get; set; }

        public ConfigEntry<bool> VersionLabelVisability { get; }

        public ConfigEntry<bool> HidePVEButton { get; set; }
        public ConfigEntry<bool> HideBetaBanner { get; set; }
        public ConfigEntry<bool> HideTopGlow { get; set; }

        public bool initOnce = false;

        public ConfigEntry<string>? mapButtonTextConfig;

        public static GameObject? locationMenuObj = null;

        public ConfigEntry<float>? CharacterZoom;

        public ConfigEntry<Color>? CustomsColorConfig { get; set; }
        public ConfigEntry<Color>? FactoryColorConfig { get; set; }
        public ConfigEntry<Color>? WoodsColorConfig { get; set; }
        public ConfigEntry<Color>? InterchangeColorConfig { get; set; }
        public ConfigEntry<Color>? ReserveColorConfig { get; set; }
        public ConfigEntry<Color>? ShorelineColorConfig { get; set; }
        public ConfigEntry<Color>? LighthouseColorConfig { get; set; }
        public ConfigEntry<Color>? GroundZeroColorConfig { get; set; }
        public ConfigEntry<Color>? StreetsColorConfig { get; set; }
        public ConfigEntry<Color>? LabsColorConfig { get; set; }


        public UIRefreshConfig(ConfigFile config)
        {
            CustomsColorConfig = config.Bind("Loading Screen Accent Colors", "Customs", new Color(0.55f, 0.55f, 0.08f), ".");
            FactoryColorConfig = config.Bind("Loading Screen Accent Colors", "Factory", new Color(0.4f, .12f, .10f), ".");
            WoodsColorConfig = config.Bind("Loading Screen Accent Colors", "Woods", new Color(0.01f, 0.36f, 0.16f), ".");
            InterchangeColorConfig = config.Bind("Loading Screen Accent Colors", "Interchange", new Color(0.01f, 0.34f, 1), ".");
            ReserveColorConfig = config.Bind("Loading Screen Accent Colors", "Reserve", new Color(0.49f, 0.06f, 0.01f), ".");
            ShorelineColorConfig = config.Bind("Loading Screen Accent Colors", "Shoreline", new Color(0.43f, 0.19f, 0.43f), ".");
            LighthouseColorConfig = config.Bind("Loading Screen Accent Colors", "Lighthouse", new Color(0.90f, 0.6f, 0.13f), ".");
            GroundZeroColorConfig = config.Bind("Loading Screen Accent Colors", "Ground Zero", new Color(0.53f, 0.6f, 0.67f), ".");
            StreetsColorConfig = config.Bind("Loading Screen Accent Colors", "Streets", new Color(0.55f, .5f, 0.5f), ".");
            LabsColorConfig = config.Bind("Loading Screen Accent Colors", "Labs", new Color(1, 1, 1), ".");


            ClockUsesSystemTimeConfig = config.Bind("General", "Clock Uses System Time", false, "Have the clock widget use your system time.");
            MenuLayoutChangesConfig = config.Bind("General", "Menu Layout Changes", true, "Enables the various edits to the layouts of menus.");
            SkipPreRaidMenusConfig = config.Bind("z. Beta", "Skip Pre-Raid Menus", false, "Skips Raid Settings and Insurance Menus.");
            EnableClockPatchConfig = config.Bind("General", "Enable Clock Widget", true, "Enable or disable the raid clock widget.");


            HideStanceSillhouette = config.Bind("HUD", "Hide Stance Silhouette", true, "Hides the Stance Silhouette.");
            HideStanceSillhouette.SettingChanged += delegate (object sender, EventArgs e)
            {
                BattleStancePanel_ShowPatch.StanceSillhouetteUpdate();
            };

            mapOnTaskBarConfig = config.Bind("General", "Enable Deploy Button", true, "Enable or disable the Deploy button on the Taskbar.");
            mapButtonTextConfig = config.Bind("General", "Deploy Button Text", "DEPOLY", "The text that appears on the Deploy button.");
            mapButtonTextConfig.SettingChanged += delegate (object sender, EventArgs e)
            {
                MenuTaskBar_AwakePatch.UpdateMapButtonText();
            };

            ChangeUISceneOnLoading = config.Bind("General", "Map Specific Loading Screen", true, "Changes the background scene when loading a raid per map.(Restart)");
            HideMenuBackgroundInRaid = config.Bind("General", "Hide Menu Background In Raid", true, "Hides the pause menu background when you are in a raid.(Restart)");
            HideOutMainMenuConfig = config.Bind("z. Beta", "Show Hideout in Main Menu", false, "Shows the Hideout in the main menu.(Restart)");

            VersionLabelVisability = config.Bind("General", "Hide Version Label", true, "Hides the version label in the bottom left.");
            VersionLabelVisability.SettingChanged += delegate (object sender, EventArgs e)
            {
                HideVersionLabelPatch.UpdateVersionLabel();
            };

            HidePVEButton = config.Bind("General", "Hide PVE Button", true, "Hides the PVE Button in the bottom right.");
            HidePVEButton.SettingChanged += delegate (object sender, EventArgs e)
            {
                HidePVEButton_Patch.UpdateGameModeButton();
            };

            HideBetaBanner = config.Bind("General", "Hide Beta Warning", true, "Hides the orange banner in the main menu.");
            HideBetaBanner.SettingChanged += delegate (object sender, EventArgs e)
            {
                HideBetaBanner_Patch.UpdateBetaBanner();
            };

            HideTopGlow = config.Bind("General", "Hide Top Glow", true, "Hides the yellow glow in the menu.");
            HideTopGlow.SettingChanged += delegate (object sender, EventArgs e)
            {
                HideTopGlowPatch.UpdateTopGlow();
            };

            MoveHealthPanelConfig = config.Bind("HUD", "Move Health Panel", true, "Moves the character health panel to the bottom left.");
            MoveHealthPanelConfig.SettingChanged += delegate (object sender, EventArgs e)
            {
                CharacterHealthPanel_Patch.MoveCharacterHealthPanel();
            };

            DisableGroupConfig = config.Bind("General", "Hide Group Widget", true, "Hides the Group button on the Task Bar.");
            new HideGroupPanel_Patch().Enable();
            DisableGroupConfig.SettingChanged += delegate (object sender, EventArgs e)
            {
                HideGroupPanel_Patch.UpdateGroupPanel();
            };
            HideGesturesQuickPanel = config.Bind("HUD", "Hide Phrase Prompt", true, "Hides the contextual phrase prompt in raid.");
            new HideGroupPanel_Patch().Enable();
            HideGesturesQuickPanel.SettingChanged += delegate (object sender, EventArgs e)
            {
                GesturesQuickPanel_ShowPatch.HideGesturesQuickPanelUpdate();
            };
            HideRaidTimerWarning = config.Bind("HUD", "Hide Raid Timer Warning", true, "Hides the <10 minute raid timer");
            new HideGroupPanel_Patch().Enable();
            HideRaidTimerWarning.SettingChanged += delegate (object sender, EventArgs e)
            {
                HideGroupPanel_Patch.UpdateGroupPanel();

            };
            HideBackpackInventory = config.Bind("HUD", "Hide Backpack Inventory", true, "Makes the backpack inaccessable when on your back.");
            new HideGroupPanel_Patch().Enable();
            HideBackpackInventory.SettingChanged += delegate (object sender, EventArgs e)
            {
                HideGroupPanel_Patch.UpdateGroupPanel();
            };
            HideAmmoPanel = config.Bind("HUD", "Hide Ammo Panel", true, "Hides the range of your sight in the bottom right.");
            new HideGroupPanel_Patch().Enable();
            HideAmmoPanel.SettingChanged += delegate (object sender, EventArgs e)
            {
                AmmoPannel_ShowPatch.AmmoPanelUpdate();
            };
        }
    }
}
