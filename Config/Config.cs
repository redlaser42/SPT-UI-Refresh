using BepInEx.Configuration;
using System;
using UIRefresh.Patches;
using UnityEngine;
using static Fika.Core.UI.FikaUIGlobals;

namespace UIRefresh.Config
{
    public class UIRefreshConfig
    {
        public ConfigEntry<bool> EnableClockPatchConfig { get; set; }
        public ConfigEntry<bool> ClockUsesSystemTimeConfig { get; set; }
        public ConfigEntry<bool> HideStanceSillhouette { get; set; }
        public ConfigEntry<float> GesturesQuickPanelAlpha { get; set; }
        public ConfigEntry<bool> HideRaidTimerWarning { get; set; }
        public ConfigEntry<bool> DisableStanceSlider { get; set; }
        public ConfigEntry<bool> DisableNoiseLevel { get; set; }
        public ConfigEntry<bool> DisableSpeedSlider { get; set; }


        public ConfigEntry<float> AmmoPanelAlpha { get; set; }

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

        public ConfigEntry<string>? mapButtonTextConfig { get; set; }

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
            ColorUtility.TryParseHtmlString("#CEB014FF", out Color customsColor);
            ColorUtility.TryParseHtmlString("#923700FF", out Color factoryColor);
            ColorUtility.TryParseHtmlString("#3A6724FF", out Color woodsColor);
            ColorUtility.TryParseHtmlString("#0F2AA4FF", out Color interchangeColor);
            ColorUtility.TryParseHtmlString("#8C0000FF", out Color reserveColor);
            ColorUtility.TryParseHtmlString("#B75B8CFF", out Color shorelineColor);
            ColorUtility.TryParseHtmlString("#E68721FF", out Color lighthouseColor);
            ColorUtility.TryParseHtmlString("#69B1DBFF", out Color groundZeroColor);
            ColorUtility.TryParseHtmlString("#6D9280FF", out Color streetsColor);
            ColorUtility.TryParseHtmlString("#FFFFFFFF", out Color labsColor);

            CustomsColorConfig = config.Bind("Loading Screen Accent Colors", "Customs", customsColor, ".");
            FactoryColorConfig = config.Bind("Loading Screen Accent Colors", "Factory", factoryColor, ".");
            WoodsColorConfig = config.Bind("Loading Screen Accent Colors", "Woods", woodsColor, ".");
            InterchangeColorConfig = config.Bind("Loading Screen Accent Colors", "Interchange", interchangeColor, ".");
            ReserveColorConfig = config.Bind("Loading Screen Accent Colors", "Reserve", reserveColor, ".");
            ShorelineColorConfig = config.Bind("Loading Screen Accent Colors", "Shoreline", shorelineColor, ".");
            LighthouseColorConfig = config.Bind("Loading Screen Accent Colors", "Lighthouse", lighthouseColor, ".");
            GroundZeroColorConfig = config.Bind("Loading Screen Accent Colors", "Ground Zero", groundZeroColor, ".");
            StreetsColorConfig = config.Bind("Loading Screen Accent Colors", "Streets", streetsColor, ".");
            LabsColorConfig = config.Bind("Loading Screen Accent Colors", "Labs", labsColor, ".");


            ClockUsesSystemTimeConfig = config.Bind("General", "Clock Uses System Time", false, "Have the clock widget use your system time.");
            MenuLayoutChangesConfig = config.Bind("General", "Menu Layout Changes", true, "Enables the various edits to the layouts of menus.");
            SkipPreRaidMenusConfig = config.Bind("General", "Skip Pre-Raid Menus", false, "Skips Raid Settings and Insurance Menus.");
            EnableClockPatchConfig = config.Bind("General", "Enable Clock Widget", true, "Enable or disable the raid clock widget.");


            HideStanceSillhouette = config.Bind("HUD", "Hide Stance Silhouette", true, "Hides the Stance Silhouette.");
            HideStanceSillhouette.SettingChanged += delegate (object sender, EventArgs e)
            {
                BattleStancePanel_ShowPatch.StanceSillhouetteUpdate();
            };

            mapOnTaskBarConfig = config.Bind("General", "Enable Deploy Button", true, "Enable or disable the Deploy button on the Taskbar.");
            mapButtonTextConfig = config.Bind("General", "Location Select Button Text", "DEPLOY", "The text that appears on the Deploy button.");
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

            GesturesQuickPanelAlpha = config.Bind("HUD", "Phrase Prompt Opacity", 0.08f, new ConfigDescription("Fades the contextual phrase prompt in raid", new AcceptableValueRange<float>(0.0f, 1.0f)));
            GesturesQuickPanelAlpha.SettingChanged += delegate (object sender, EventArgs e)
            {
                GesturesQuickPanel_ShowPatch.UpdateGesturesQuickPanel();
            };

            HideRaidTimerWarning = config.Bind("z. Beta", "Hide Raid Timer Warning", true, "Does not work. Hides the <10 minute raid timer");
            HideRaidTimerWarning.SettingChanged += delegate (object sender, EventArgs e)
            {
            };
            AmmoPanelAlpha = config.Bind("HUD", "Hide Ammo Panel", 0.1f, new ConfigDescription("Hides the range of your sight in the bottom right.", new AcceptableValueRange<float>(0.0f, 1.0f)));
            AmmoPanelAlpha.SettingChanged += delegate (object sender, EventArgs e)
            {
                AmmoPannel_ShowPatch.AmmoPanelUpdate();
            };
            DisableNoiseLevel = config.Bind("HUD", "Disable Noise Level", true, "Hides the Noise Level indicator on the BattleHUD");
            DisableNoiseLevel.SettingChanged += delegate (object sender, EventArgs e)
            {
                AmmoPannel_ShowPatch.AmmoPanelUpdate();
            };
            DisableSpeedSlider = config.Bind("HUD", "Disable Speed Slider", true, "Hides the Speed Slider on the BattleHUD");
            DisableSpeedSlider.SettingChanged += delegate (object sender, EventArgs e)
            {
                AmmoPannel_ShowPatch.AmmoPanelUpdate();
            };
            DisableStanceSlider = config.Bind("HUD", "Disable Stance Slider", true, "Hides the Stance Slider on the BattleHUD");
            DisableStanceSlider.SettingChanged += delegate (object sender, EventArgs e)
            {
                AmmoPannel_ShowPatch.AmmoPanelUpdate();
            };
        }
    }
}
