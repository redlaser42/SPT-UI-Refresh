using BepInEx;
using UIRefresh.Config;
using UIRefresh.Patches;

namespace UIRefresh
{
    [BepInPlugin("redlaser42.UIRefresh", "UI Refresh", "2.3.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; } = null!;
        public UIRefreshConfig UIRefreshConfig { get; private set; } = null!;

        private void Awake()
        {
            UIRefreshConfig = new UIRefreshConfig(Config);
            Instance = this;

            new HideVersionLabelPatch().Enable();
            new HidePVEButton_Patch().Enable();
            new HideBetaBanner_Patch().Enable();
            new HideTopGlowPatch().Enable();
            new HideGroupPanel_Patch().Enable();

            //Add Map to Taskbar
            new MenuTaskBar_AwakePatch().Enable();

            //Clock Patch
            new InventoryScreen_ShowPatch().Enable();

            //Filter Inventory Shortcut
            //new FilterPanel_Show().Enable();

            //Load hideout if needed.
            new MenuScreen_ShowPatch().Enable();

            //Select SCAV or PMC
            new SideSelection_ShowPatch().Enable();
            //Location Selection Map
            new LocationSelection_ShowPatch().Enable();
            //Pre-Raid Settings
            new RaidSettingsScreen_ShowPatch().Enable();
            //Insurance Screen
            new InsuranceScreen_ShowPatch().Enable();
            //Host or Join Menu
            new MatchMakerAcceptScreen_Patch().Enable();
            new MatchMakerAcceptScreenMethod_27_Patch().Enable();
            //Loading Screen
            new TimeHasCome_ShowPatch().Enable();
            //Deploying... screen
            new FinalCountdown_ShowPatch().Enable();

            //HUD edits
            new CharacterHealthPanel_Patch().Enable();
            new BattleStancePanel_ShowPatch().Enable();

            new GesturesQuickPanel_ShowPatch().Enable();
            //new MainTimerPanel_ShowPatch().Enable();

            new AmmoPannel_ShowPatch().Enable();

            //Exiting Raid
            new SessionResultExitStatus_ShowPatch().Enable();

            //new HideoutScreenOverlay_Show().Enable();

            Utils.checkFika();

            if (Utils.playingFika)
            {
                //new FikaCreateMainMenuUI_Patch();
            }
        }
    }   
}
