using BepInEx;
using BepInEx.Configuration;
using System;
using UIRefresh.Patches;
using UnityEngine;

namespace UIRefresh
{
    [BepInPlugin("com.redlaser42.UI_Refresh", "redlaser42.UI_Refresh", "2.0.0")]
    public class Plugin : BaseUnityPlugin
    {

    public static ConfigEntry<bool>? EnableClockPatchConfig;
    public static ConfigEntry<bool>? ClockUsesSystemTimeConfig;
    public static ConfigEntry<bool>? HideQuickSlotsConfig;
    public static ConfigEntry<bool>? ShowStanceSillhouette;
    public static ConfigEntry<bool>? HideBackgoundpPatch;
    public static ConfigEntry<bool>? DisableGroupConfig;
    public static ConfigEntry<bool>? HideOutMainMenuConfig;
    public static ConfigEntry<bool>? SkipPreRaidMenusConfig;
    public static ConfigEntry<bool>? MenuLayoutChangesConfig;
    public static ConfigEntry<bool>? ChangeUISceneOnLoading;
    public static ConfigEntry<bool>? HideMenuBackgroundInRaid;
    public static ConfigEntry<bool>? mapOnTaskBarConfig;
    public static ConfigEntry<bool>? VersionLabelVisability;
    public static ConfigEntry<bool>? HideGameModeButton;
    public static ConfigEntry<bool>? HideBetaBanner;
    public static ConfigEntry<bool>? HideTopGlow;


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
        DisableGroupConfig = Config.Bind("General", "c. Disable Group Widget",true,"Disables the Group buttons on the Task Bar.");
        ClockUsesSystemTimeConfig = Config.Bind("General", "e. Clock Uses System Time", false,"Have the clock widget use your system time.");
        MenuLayoutChangesConfig = Config.Bind("General", "a. Menu Layout Changes", true, "Enables the various edits to the layouts of menus.");
        if(MenuLayoutChangesConfig.Value)
        {
        }

        SkipPreRaidMenusConfig = Config.Bind("z. Beta", "b. Skip Pre-Raid Menus", false, "Skips Raid Settings and Insurance Menus.");
        EnableClockPatchConfig = Config.Bind("General", "d. Enable Clock Widget", true, "Enable or disable the raid clock widget.");
        if (EnableClockPatchConfig.Value)
        {
            new InventoryScreen_ShowPatch().Enable();
        }


        ShowStanceSillhouette = Config.Bind("2. HUD", "b. Show Stance Guy", false, "Hides/Shows the Stance Silhouette.(Restart)");
        mapOnTaskBarConfig = Config.Bind("General", "f. Enable Map Button", true, "Enable or disable the Map button on the Taskbar.");
        mapButtonTextConfig = Config.Bind("General", "c. Map button Text", "MAP", "The text that appears on the Map button.(Restart)");
        new MenuTaskBar_AwakePatch().Enable();


        new BattleStancePanel_ShowPatch().Enable();
        ShowStanceSillhouette.SettingChanged += delegate (object sender, EventArgs e)
        {
        BattleStancePanel_ShowPatch.StanceSillhouetteUpdate();
        };


        ChangeUISceneOnLoading = Config.Bind("General", "g. Map Specific Loading Screen", true, "Changes the background scene when loading a raid per map.(Restart)");
        HideMenuBackgroundInRaid = Config.Bind("General", "b. Hide Menu Background In Raid", true, "Hides the pause menu background when you are in a raid.(Restart)");
        HideOutMainMenuConfig = Config.Bind("z. Beta", "a. Show Hideout in Main Menu", false, "Shows the Hideout in the main menu.(Restart)");
        if (HideOutMainMenuConfig.Value)
        {
            new HideoutOverlay_ShowPatch().Enable();
        }

        VersionLabelVisability = Config.Bind("General", "Show Version Label", true, "Hides/Shows the version label in the bottom left.");
        new HideVersionLabelPatch().Enable();
        VersionLabelVisability.SettingChanged += delegate (object sender, EventArgs e)
        {
            HideVersionLabelPatch.UpdateVersionLabel();
        };

        HideGameModeButton = Config.Bind("General", "Show PVE Button", false, "Hides/Shows the PVE Button in the bottom right.");
        new HideGameModePatch().Enable();
        HideGameModeButton.SettingChanged += delegate (object sender, EventArgs e)
        {
            HideGameModePatch.UpdateGameModeButton();
        };


        HideBetaBanner = Config.Bind("General", "Show Beta Warning", false, "Hides/Shows the orange banner in the main menu.");
        new BetaBannerVisability_Patch().Enable();
    
        HideBetaBanner.SettingChanged += delegate (object sender, EventArgs e)
        {
            BetaBannerVisability_Patch.UpdateBetaBanner();
        };


        HideTopGlow = Config.Bind("General", "Show Top Glow", false, "Hides/Shows the yellow glow in the menu.");
        new HideTopGlowPatch().Enable();
        HideTopGlow.SettingChanged += delegate (object sender, EventArgs e)
        {
            HideTopGlowPatch.UpdateTopGlow();
        };

        Utils.checkFika();
        if (Utils.playingFika)
        {
            //new FIKA_OnlinePlayers_Patch().Enable();
        }
        new MenuScreen_ShowPatch().Enable();
        new SideSelection_ShowPatch().Enable();
        new LocationSelection_ShowPatch().Enable();
        new RaidSettingsScreen_ShowPatch().Enable();
        new InsuranceScreen_ShowPatch().Enable();
        // new MatchMakerAcceptScreen_Patch().Enable();
        new TimeHasCome_ShowPatch().Enable();
        new FinalCountdown_ShowPatch().Enable();
        new SessionResultExitStatus_ShowPatch().Enable();
        }

    }   
}
