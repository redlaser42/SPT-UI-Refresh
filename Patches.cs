using BepInEx.Logging;
using EFT;
using EFT.HealthSystem;
using EFT.InventoryLogic;
using EFT.UI;
using EFT.UI.Matchmaker;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UIRefresh.Patches
{
  
    //SCAV or PMC Selection Menu
    internal class SideSelectionPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchMakerSideSelectionScreen), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(IHealthController), typeof(InventoryController)});
        }

        [PatchPostfix]
        static void Postfix(MatchMakerSideSelectionScreen __instance)
        {
            // Deactivates Logo and texts.
            __instance.transform.Find("CaptionsHolder").gameObject.SetActive(false);
            __instance.transform.Find("Logo").gameObject.SetActive(false);
            __instance.transform.Find("Description").gameObject.SetActive(false);

            // Adjust PMC model
            var PMC = __instance.transform.Find("PMCs");
                PMC.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, -190f);
                PMC.transform.Find("PMCPlayerMV").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 150);
                PMC.transform.Find("PMCPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Camera>().fieldOfView = 20;
            PMC.transform.Find("PMCPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Transform>().localPosition = new Vector3(0.62f, -0.4f, 0.96f);
            PMC.transform.Find("PMCPlayerMV").GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 0);

            //Adjust SCAV model
            var SCAV = __instance.transform.Find("Savage");
            SCAV.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, -190f);
            SCAV.transform.Find("ScavPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Camera>().fieldOfView = 20;
            SCAV.transform.Find("ScavPlayerMV").GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 0);
            SCAV.transform.Find("ScavPlayerMV").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 150);
            SCAV.transform.Find("ScavPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Transform>().localPosition = new Vector3(-1.71f, -0.39f, 1.28f);

        }
    }

    // Level Select Map Menu
    internal class LocationSelectionPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchMakerSelectionLocationScreen), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(MatchmakerPlayerControllerClass)});
        }

        [PatchPostfix]
        static void Postfix(MatchMakerSelectionLocationScreen __instance)
        {
            //Deactivates top text and stripped background of condidions panel.
            __instance.transform.Find("CaptionsHolder").gameObject.SetActive(false);
            __instance.transform.Find("Conditions Panel").Find("Tiles").gameObject.SetActive(false);

            // Centers Map.
            __instance.transform.Find("Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 0);

            //Increases location name and icon. 
            __instance.transform.Find("Content").Find("Location Info Panel").GetComponent<RectTransform>().anchoredPosition = new Vector2(100, -160);
            __instance.transform.Find("Content").Find("Location Info Panel").Find("Banner").GetComponent <RectTransform>().localScale = new Vector3(1.76f, 1.76f , 1);
        }
    }

    //Offline Raid Settings Menu
    internal class RaidSettingsScreenPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchmakerOfflineRaidScreen), "Show", new System.Type[] { typeof(InfoClass), typeof(RaidSettings), typeof(RaidSettings) });
        }

        [PatchPostfix]
        static void Postfix(MatchmakerOfflineRaidScreen __instance)
        {
            // Deactivates top text and spacers.
            __instance.transform.Find("Content").Find("Description").gameObject.SetActive(false);
            __instance.transform.Find("Content").Find("Space (1)").gameObject.SetActive(false);
            __instance.transform.Find("Content").Find("Space (2)").gameObject.SetActive(false);
        }
    }

    //Insurance Menu
    internal class InsuranceScreenPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchmakerInsuranceScreen), "Show", new System.Type[] { typeof(IHealthController), typeof(InventoryController), typeof(ISession) });
        }

        [PatchPostfix]
        static void Postfix(MatchmakerInsuranceScreen __instance)
        {
            __instance.transform.Find("WarningPanel").gameObject.SetActive(false);
            __instance.transform.Find("Tab Bar").GetComponent<RectTransform>().anchoredPosition = new Vector2(1, -100);

            __instance.transform.Find("ItemsToInsurePanel").GetComponent<RectTransform>().sizeDelta = new Vector2(5, -305);
            __instance.transform.Find("ItemsToInsurePanel").GetComponent<RectTransform>().anchoredPosition = new Vector2(-3.5f, 20);
            __instance.transform.Find("ItemsToInsurePanel").Find("ItemsToInsureList").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -80);
            __instance.transform.Find("ItemsToInsurePanel").Find("ItemsToInsureList").GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 490);
        }
    }

    //Accept Location Menu
    internal class AcceptLocationPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchMakerAcceptScreen), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(RaidSettings)});
        }

        [PatchPostfix]
        static void Postfix(MatchMakerAcceptScreen __instance)
        {
            // Deactiavtes text on top and time/weather pannel.
            __instance.transform.Find("CaptionsHolder").gameObject.SetActive(false);
            __instance.transform.Find("Conditions Panel").gameObject.SetActive(false);

            // Adjust player model
            var previewPanel = __instance.transform.Find("PreviewsPanel");
                previewPanel.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, 155f);
                previewPanel.transform.Find("CurrentPlayerModelView").Find("PlayerMVObject").Find("Camera_acceptScreen").GetComponent<Camera>().fieldOfView = 12;
                previewPanel.transform.Find("CurrentPlayerModelView").Find("PlayerMVObject").Find("Camera_acceptScreen").GetComponent<Transform>().localEulerAngles = new Vector3(358f, 352.3f, 0f);
                previewPanel.transform.Find("CurrentPlayerModelView").Find("PlayerMVObject").Find("Camera_acceptScreen").GetComponent<Transform>().localPosition = new Vector3(0.81f, - 0.08f, - 1.8f);
                previewPanel.transform.Find("CurrentPlayerModelView").Find("PlayerMVObject").Find("Camera_acceptScreen").GetComponent<PrismEffects>().enabled = false;
                previewPanel.transform.Find("CurrentPlayerModelView").GetComponent<RectTransform>().sizeDelta = new Vector2(600, 1000);
                previewPanel.transform.Find("CurrentPlayerModelView").GetComponent<RectTransform>().anchoredPosition = new Vector2(-18, -300);
        }
    }

    //Raid Loading Screen
    internal class TimeHasComePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchmakerTimeHasCome), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(MatchmakerPlayerControllerClass) });
        }

        [PatchPostfix]
        static void Postfix(MatchmakerBannersPanel ____bannersPanel, PlayerModelView ____playerModelView, MatchmakerTimeHasCome __instance)
        {
            //Deactivate banner, logo, tips, and loading spinner.
            ____bannersPanel.gameObject.SetActive(false);
            __instance.transform.Find("CaptionsHolder").gameObject.SetActive(false);
            __instance.transform.Find("Logo").gameObject.SetActive(false);
            __instance.transform.Find("Loader").gameObject.SetActive(false);

            //Other Players in Raid list - move
            __instance.transform.Find("PartyInfoPanel").GetComponent<RectTransform>().localPosition = new Vector2(-922, 550);

            //Map Name - move, increase font, set background to match.
            var locationName = __instance.transform.Find("Location Name Panel");
                locationName.gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-880,950);
                locationName.gameObject.transform.Find("Name").GetComponent<CustomTextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
                locationName.gameObject.transform.Find("Name").GetComponent<CustomTextMeshProUGUI>().fontSize = 130;
                locationName.gameObject.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(450, 150);
                locationName.gameObject.transform.Find("Background").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            //Loading Status Text - move and align left
            var deployCaption = __instance.transform.Find("Deploying Caption");
                deployCaption.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-746, 17);
                deployCaption.gameObject.GetComponent<CustomTextMeshProUGUI>().alignment = TextAlignmentOptions.Left;

            //Start and Back Button - move
                __instance.transform.Find("Back Button Panel").gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-840, 55);

            //Player Model - move and resize.
            var playerModel = __instance.transform.Find("PlayerModelView");
                playerModel.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(800f, 0f);
                playerModel.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 0f);
                playerModel.gameObject.GetComponent<AspectRatioFitter>().aspectRatio = 2.7f;


            //Player Model Lights. Deactiavate all but Main and adjust.
            playerModel.Find("PlayerModelViewObject").Find("Camera_timehascome0").GetComponent<Camera>().fieldOfView = 26;
                var lights = playerModel.Find("PlayerModelViewObject").Find("Lights");
                for (int i = 0; i < lights.childCount; i++)
                {
                    Transform child = lights.GetChild(i);
                    child.gameObject.SetActive(child.name == "Main Light");
                }

                var mainLight = lights.Find("Main Light");
                    mainLight.GetComponent<Transform>().localEulerAngles = new Vector3 (60f, 60f, 20f);
                    mainLight.GetComponent<Transform>().localPosition = new Vector3(-0.243f, -294.1938f, 4.65f);
                    mainLight.GetComponent<Light>().type = LightType.Spot;
                    mainLight.GetComponent<Light>().spotAngle = 65;
                    mainLight.GetComponent<Light>().range = 1.3f;
                    mainLight.GetComponent<Light>().intensity = 6f;
                    mainLight.GetComponent<Light>().color = new Color(1, 0.86f, 0.74f, 1);

            //Remove Bottom Task Bar except for Settings button.
            PreloaderUI preloaderUI = MonoBehaviourSingleton<PreloaderUI>.Instance;

            if (preloaderUI == null)
            {
                Logger.LogError("preloaderUI Null");
                return;
            }
            Logger.LogWarning("Attempted to hide Taskbar");
            preloaderUI.transform.Find("Preloader UI").Find("BottomPanel").Find("Background").gameObject.SetActive(false);
            var taskbarTabs = preloaderUI.transform.Find("Preloader UI").Find("BottomPanel").Find("Content").Find("TaskBar").Find("Tabs");

            for (int i = 0; i < taskbarTabs.childCount; i++)
            {
                Transform child = taskbarTabs.GetChild(i);
                child.gameObject.SetActive(child.name == "Settings");
            }
        }
    }

    // "Deploying In.." Screen
    internal class FinalCountdownPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchmakerFinalCountdown), "Show", new System.Type[] { typeof(Profile), typeof(DateTime)});
        }

        [PatchPostfix]
        static void Postfix(MatchmakerFinalCountdown __instance)
        {
            __instance.transform.Find("Logo").gameObject.SetActive(false);
        }
    }

    // Add Clock to Inventory Screen
    internal class ClockPatch : ModulePatch
    {

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(InventoryScreen), "Show", new System.Type[] { typeof(IHealthController), typeof(InventoryController), typeof(AbstractQuestControllerClass), typeof(AbstractAchievementControllerClass), typeof(GClass3695), typeof(CompoundItem),typeof(EInventoryTab), typeof(ISession), typeof(ItemContextAbstractClass), typeof(Boolean) });
        }

        [PatchPostfix]
        public static void Postfix(InventoryScreen __instance, ISession ___iSession)
        {
            var existingClock = __instance.transform.Find("Clock Widget");
            if (existingClock != null)
            {
                var clockText = existingClock.GetComponent<TMPro.TextMeshProUGUI>();
                clockText.text = ___iSession.GetCurrentLocationTime.ToString("HH:mm:ss");
                return;
            }

            var clockWidget = new GameObject("Clock Widget");
            clockWidget.transform.SetParent(__instance.transform, false);
            var newClockText = clockWidget.AddComponent<TMPro.TextMeshProUGUI>();
            newClockText.text = ___iSession.GetCurrentLocationTime.ToString("HH:mm:ss");
            newClockText.fontSize = 29;
            clockWidget.GetComponent<RectTransform>().anchoredPosition = new Vector2(670, 510);
        }
    }
}