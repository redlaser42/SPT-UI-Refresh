using BepInEx.Logging;
using BepInEx;
using System;
using UIRefresh.Patches;

namespace UIRefresh
{
        [BepInPlugin("redlaser42.UI_Refresh", "redlaser42.UI_Refresh", "1.0.0")]
        public class Plugin : BaseUnityPlugin
        {
            public static ManualLogSource? LogSource;

            private void Awake()
            {
                LogSource = Logger;
                LogSource.LogInfo("UI Refresh loaded!");

            new SideSelectionPatch().Enable();
            new LocationSelectionPatch().Enable();
            new RaidSettingsScreenPatch().Enable();
            new InsuranceScreenPatch().Enable();
            new AcceptLocationPatch().Enable();
            new TimeHasComePatch().Enable();
            new FinalCountdownPatch().Enable();
            new ClockPatch().Enable();
            new MapOnTaskBarPatch().Enable();

        }
    }
    }
