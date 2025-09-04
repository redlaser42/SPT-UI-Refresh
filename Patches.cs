using BepInEx.Bootstrap;
using Comfort.Common;
using EFT;
using EFT.HealthSystem;
using EFT.InventoryLogic;
using EFT.UI;
using EFT.UI.Matchmaker;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using UIRefresh;


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
                PMC.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, -100);
                PMC.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 300);

            //PMC.transform.Find("PMCPlayerMV").GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 500);
            //PMC.transform.Find("PMCPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Camera>().fieldOfView = 20;
            //PMC.transform.Find("PMCPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Transform>().localPosition = new Vector3(0.62f, 0.4f, 0.96f);
            //PMC.transform.Find("PMCPlayerMV").GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 0);
            //PMC.transform.Find("PMCPlayerMV").Find("PlayerMVObject").Find("MenuPlayer").GetComponent<Transform>().localPosition = new Vector3(0.62f, 0.4f, 0.96f);


            //Adjust SCAV model
            var SCAV = __instance.transform.Find("Savage");
            SCAV.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -100);
            SCAV.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 300);

            //SCAV.transform.Find("ScavPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Camera>().fieldOfView = 20;
            //SCAV.transform.Find("ScavPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Transform>().localPosition = new Vector3(-1.71f, 0.16f, 1.28f);
            //SCAV.transform.Find("ScavPlayerMV").GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 0);
            //SCAV.transform.Find("ScavPlayerMV").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 320);
            //SCAV.transform.Find("ScavPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Transform>().localEulerAngles = new Vector3(0, 200, 0);

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

            var conditionsPannel = __instance.transform.Find("Conditions Panel");
                conditionsPannel.Find("Tiles").gameObject.SetActive(false);
                conditionsPannel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-440, 860);
                conditionsPannel.GetComponent<Image>().enabled = false;

            var map = __instance.transform.Find("Content").GetChild(1);
            var locationInfo = __instance.transform.Find("Content").GetChild(0);

            //Increases location name and icon. 
            locationInfo.GetComponent<RectTransform>().anchoredPosition = new Vector2(-800, -200);
            locationInfo.Find("Banner").GetComponent <RectTransform>().localScale = new Vector3(1.76f, 1.76f , 1);
            locationInfo.Find("DescriptionPanel").GetChild(0).GetComponent<CustomTextMeshProUGUI>().fontSize = 16;
            locationInfo.Find("DescriptionPanel").GetChild(1).GetChild(2).gameObject.SetActive(false);

            map.GetComponent<RectTransform>().anchoredPosition = new Vector2(950, 100);
            map.GetComponent<RectTransform>().sizeDelta = new Vector2(796, 0);

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
            //__instance.transform.Find("Conditions Panel").gameObject.SetActive(false);

            // Adjust player model
            var previewPanel = __instance.transform.Find("PreviewsPanel");
            if (previewPanel != null)
            {
                //previewPanel.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-530, 155f);
                //previewPanel.transform.Find("CurrentPlayerModelView").Find("PlayerMVObject").Find("Camera_acceptScreen").GetComponent<Camera>().fieldOfView = 18;
                previewPanel.transform.Find("CurrentPlayerModelView").Find("PlayerMVObject").Find("Camera_acceptScreen").GetComponent<Transform>().localEulerAngles = new Vector3(358f, 352.3f, 0f);
                previewPanel.transform.Find("CurrentPlayerModelView").Find("PlayerMVObject").Find("Camera_acceptScreen").GetComponent<Transform>().localPosition = new Vector3(0.81f, -0.08f, -1.8f);
                //previewPanel.transform.Find("CurrentPlayerModelView").GetComponent<RectTransform>().sizeDelta = new Vector2(600, 1300);
                //previewPanel.transform.Find("CurrentPlayerModelView").GetComponent<RectTransform>().anchoredPosition = new Vector2(-18, -300);
            }
        }
    }

    //Raid Loading Screen
    internal class TimeHasComeShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchmakerTimeHasCome), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(MatchmakerPlayerControllerClass) });
        }

        [PatchPostfix]
        static void Postfix(MatchmakerBannersPanel ____bannersPanel, PlayerModelView ____playerModelView, MatchmakerTimeHasCome __instance)
        {
            var locationNamePanel = __instance.transform.Find("Location Name Panel");
            var locationNameGUI = locationNamePanel.gameObject.transform.Find("Name").GetComponent<CustomTextMeshProUGUI>();

            var accentColor = Plugin.GetMapColor(locationNameGUI.text);

            //Deactivate banner, logo, tips, and loading spinner.
            var bannerPanel = ____bannersPanel.gameObject;
                if (bannerPanel != null)
                {
                bannerPanel.gameObject.SetActive(false); 
                }

            var captionsHolder = __instance.transform.Find("CaptionsHolder");
            if (captionsHolder != null)
            {
                captionsHolder.gameObject.SetActive(false);
            }

            var logo = __instance.transform.Find("Logo");
            if(logo != null)
            {
                logo.gameObject.SetActive(false);
            }

            var loader = __instance.transform.Find("Loader");
                loader.gameObject.SetActive(false);


            //Other Players in Raid list - move
            var partyInfoPanel = __instance.transform.Find("PartyInfoPanel");
            if (partyInfoPanel != null)
            {
                partyInfoPanel.GetComponent<RectTransform>().localPosition = new Vector2(-922, 550);
            }
            //Map Name - move, increase font, set background to match.
            if (locationNamePanel != null)
            {
                locationNamePanel.gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-850, 950);


                locationNameGUI.alignment = TextAlignmentOptions.Center;
                locationNameGUI.fontSize = 130;

                var locationNameText = locationNameGUI.text;

                locationNameGUI.color = accentColor;

                locationNamePanel.gameObject.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(450, 150);
                locationNamePanel.gameObject.transform.Find("Background").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            }

            //Loading Status Text transform and alignment:
            var deployCaption = __instance.transform.Find("Deploying Caption");
            if (deployCaption != null)
            {
                deployCaption.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-746, 17);
                deployCaption.gameObject.GetComponent<CustomTextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
            }

            //Start/Back Button transform:
            var startBackButton = __instance.transform.Find("Back Button Panel");
            if (startBackButton != null)
            {
                startBackButton.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-840, 55);
            }

            //Player Model transform and lights:
            var playerModel = __instance.transform.Find("PlayerModelView");
            if (playerModel != null)
            {

                playerModel.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(665f, 0f);
                playerModel.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 0f);
                playerModel.gameObject.GetComponent<AspectRatioFitter>().aspectRatio = 3.3f;

                var playerModelCamera = playerModel.Find("PlayerModelViewObject").Find("Camera_timehascome0");
                if (playerModelCamera != null)
                {
                    playerModelCamera.GetComponent<Camera>().fieldOfView = 28;
                }

                //Player Model Lights:
                var playerModellights = playerModel.Find("PlayerModelViewObject").Find("Lights");
                if (playerModellights != null)
                {
                    playerModellights.GetChild(2).gameObject.SetActive(false);
                    playerModellights.GetChild(3).gameObject.SetActive(false);

                    playerModellights.GetChild(1).GetComponent<Light>().color = accentColor;

                    var mainLight = playerModellights.Find("Main Light");
                    mainLight.GetComponent<Transform>().localEulerAngles = new Vector3(60f, 60f, 20f);
                    mainLight.GetComponent<Transform>().localPosition = new Vector3(-0.243f, -294.1938f, 4.65f);
                    mainLight.GetComponent<Light>().type = LightType.Spot;
                    mainLight.GetComponent<Light>().spotAngle = 65;
                    mainLight.GetComponent<Light>().range = 1.3f;
                    mainLight.GetComponent<Light>().intensity = 6f;
                    mainLight.GetComponent<Light>().color = new Color(1, 0.86f, 0.74f, 1);
                }
            }

            //Removes Bottom Task Bar
            PreloaderUI preloaderUI = MonoBehaviourSingleton<PreloaderUI>.Instance;
            if (preloaderUI == null)
            {
                Logger.LogError("preloaderUI Null");
                return;
            }
            preloaderUI.SetMenuTaskBarVisibility(false);

            //Hides WTT Menu Background Plane:
            var customPlane = Singleton<EnvironmentUI>.Instance.transform.Find("EnvironmentUISceneFactory/FactoryLayout/CustomPlane/");
            if (customPlane != null)
            {
                Logger.LogError("Enabling UI backgound");
                customPlane.gameObject.SetActive(false);
            }

        }
    }

    internal class TimeHasComeClosePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(MatchmakerTimeHasCome).GetMethod(nameof(MatchmakerTimeHasCome.OnDestroy));
        }

        [PatchPostfix]
        static void Postfix(MatchmakerTimeHasCome __instance)
        {

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


            GameObject environmentUI = GameObject.Find("Environment UI/");
            if (environmentUI != null)
            {
                var backgroundScene = environmentUI.transform.GetChild(2);
                backgroundScene.gameObject.SetActive(false);

                var customPlane = backgroundScene.GetChild(0).GetChild(3);
                customPlane.gameObject.SetActive(true);
            }
        }
    }

    // Add Clock to Inventory Screen
    internal class ClockPatch : ModulePatch
    {

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(InventoryScreen), "Show", new System.Type[] { typeof(IHealthController), typeof(InventoryController), typeof(AbstractQuestControllerClass), typeof(AbstractAchievementControllerClass), typeof(GClass3695), typeof(CompoundItem), typeof(EInventoryTab), typeof(ISession), typeof(ItemContextAbstractClass), typeof(Boolean) });
        }

        [PatchPostfix]
        public static void Postfix(InventoryScreen __instance, ISession ___iSession)
        {
            var clockParent = __instance.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1);

            //Check if widget was created before
            var existingClock = clockParent.transform.Find("Clock Widget");
            if (existingClock != null)
            {
                //If exisiting
                var clockText = existingClock.GetComponent<TMPro.TextMeshProUGUI>();
                clockText.text = GetRaidTime(___iSession);
                return;
            }
            //Create for first time
            var clockWidget = new GameObject("Clock Widget");
            clockWidget.transform.SetParent(clockParent, false);
            var newClockText = clockWidget.AddComponent<TMPro.TextMeshProUGUI>();
            newClockText.text = GetRaidTime(___iSession);
            newClockText.fontSize = 38;
            clockWidget.GetComponent<RectTransform>().anchoredPosition = new Vector2(170, -395);
        }

        private static string GetRaidTime(ISession ___iSession)
        {
            // If IDNC is installed, get raid time from helper.
            if (Chainloader.PluginInfos.ContainsKey("Jehree.ImmersiveDaylightCycle"))
            {
                return TryGetImmersiveTime();
            }

            // Otherwise fallback to default time
            return ___iSession.GetCurrentLocationTime.ToString("HH:mm:ss");
        }

        private static string TryGetImmersiveTime()
        {
            try
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
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to get IDNC time: {ex}");
            }
            return "??:??";
        }
    }

    //Add Map to Taskbar
    internal class MapOnTaskBarPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(MenuTaskBar).GetMethod(nameof(MenuTaskBar.Awake));
        }

        [PatchPostfix]
        public static void Postfix(Dictionary<EMenuType, AnimatedToggle> ____toggleButtons, Dictionary<EMenuType,
                   HoverTooltipArea> ____hoverTooltipAreas, ref GameObject[] ____newInformation)
        {
                GameObject fleaMarketGameObject = GameObject.Find("Preloader UI/Preloader UI/BottomPanel/Content/TaskBar/Tabs/FleaMarket");
                if (fleaMarketGameObject != null)
                {
                    GameObject MapButtonGameObject = GameObject.Instantiate(fleaMarketGameObject);
                    MapButtonGameObject.name = "Map";
                    MapButtonGameObject.transform.SetParent(fleaMarketGameObject.transform.parent, false);
                    MapButtonGameObject.transform.SetSiblingIndex(4);

                    GameObject MapButton = MapButtonGameObject.transform.GetChild(0).gameObject;
                    MapButton.name = "Map";

                    LocalizedText text = MapButtonGameObject.GetComponentInChildren<LocalizedText>();
                    if (text != null)
                    {
                    text.LocalizationKey = "";
                    text.method_2("Map");
                    }

                GameObject mapListObject = GameObject.Find("Common UI/Common UI/InventoryScreen/Tab Bar/Tabs/Map/Normal/Icon/");
                if (mapListObject != null)
                {
                    Image FoundmapImage = mapListObject.GetComponent<Image>();
                    Image MapButtonSprite = MapButtonGameObject.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>();

                    if (MapButtonSprite != null && FoundmapImage != null)
                    {
                        MapButtonSprite.sprite = FoundmapImage.sprite;
                    }


                    //Bind MAP button action and SFX.
                    AnimatedToggle animatedToggle = MapButtonGameObject.GetComponentInChildren<AnimatedToggle>();
                    if (animatedToggle != null)
                    {
                        animatedToggle.onValueChanged.AddListener(async (arg) =>
                        {
                            try
                            {
                                Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ButtonBottomBarClick);

                                var menuScreen = GameObject.Find("Common UI/Common UI/MenuScreen/").GetComponent<MenuScreen>();
                                var sideSelectionMenuGO = GameObject.Find("Menu UI/UI/MatchMaker Side Selection Screen/");
                                var sideSelectionInst = sideSelectionMenuGO.transform.GetComponent<MatchMakerSideSelectionScreen>();

                                    AccessTools.Field(typeof(MatchMakerSideSelectionScreen), "esideType_0").SetValue(sideSelectionInst, ESideType.Pmc);
                                    TarkovApplication.Exist(out TarkovApplication tarkovApp);
                                    if (tarkovApp != null)
                                    {
                                        var menuOperation = AccessTools.Field(tarkovApp.GetType(), "_menuOperation").GetValue(tarkovApp) as MainMenuControllerClass;
                                        if (menuOperation != null)
                                        {
                                            menuOperation.ShowScreen(EMenuType.Play, true);
                                        }
                                        sideSelectionInst.method_18();
                                    }
                            }
                            catch (Exception ex)
                            {
                            Logger.LogError(ex);
                            }
                        });
                    }
                }
            }
        }
    }


    // Hide Quickslot HUD
    internal class QuickSlotsHUDPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(InventoryScreenQuickAccessPanel), "Show", new System.Type[] { typeof(InventoryController), typeof(ItemUiContext), typeof(GamePlayerOwner), typeof(InsuranceCompanyClass) });
        }

        [PatchPostfix]
        static void Postfix(InventoryScreenQuickAccessPanel __instance)
        {
            GameObject QuickSlotObject = GameObject.Find("Common UI/Common UI/EFTBattleUIScreen Variant/QuickAccessPanel/");
            if (QuickSlotObject != null)
            {
                QuickSlotObject.transform.GetChild(0).gameObject.SetActive(false);
                QuickSlotObject.transform.GetChild(1).gameObject.SetActive(false);
            }

        }
    }

    // Hide Character Stance HUD
    internal class StanceSilhouettePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(InventoryScreenQuickAccessPanel), "Show", new System.Type[] { typeof(InventoryController), typeof(ItemUiContext), typeof(GamePlayerOwner), typeof(InsuranceCompanyClass) });
        }

        [PatchPostfix]
        static void Postfix(InventoryScreenQuickAccessPanel __instance)
        {

            GameObject BattleStanceObject = GameObject.Find("Common UI/Common UI/EFTBattleUIScreen Variant/BattleStancePanel/");
            if (BattleStanceObject != null)
            {
                BattleStanceObject.transform.GetChild(1).gameObject.SetActive(false);
                BattleStanceObject.transform.GetChild(3).gameObject.SetActive(false);
                BattleStanceObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    // Shows PTT Location on Map
    internal class PTTLocationPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchMakerAcceptScreen), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(RaidSettings) });
        }

        [PatchPostfix]
        static void Postfix(MatchMakerAcceptScreen __instance)
        {


        }
    }


}