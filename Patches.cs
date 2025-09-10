using BepInEx.Bootstrap;
using Comfort.Common;
using EFT;
using EFT.HealthSystem;
using EFT.Hideout;
using EFT.InventoryLogic;
using EFT.UI;
using EFT.UI.Matchmaker;
using EFT.UI.SessionEnd;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UIRefresh.Plugin;
using Color = UnityEngine.Color;


namespace UIRefresh.Patches
{
    //Main Menu Show Patch. Hides groups and other things
    internal class MenuScreen_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MenuScreen), "Show", new System.Type[] { typeof(Profile), typeof(MatchmakerPlayerControllerClass), typeof(ESessionMode) });
        }

        [PatchPostfix]
        static void Postfix(MenuScreen __instance, EnvironmentUI ___environmentUI_0)
        {

            // Show Hideout in Main Menu
            if (Plugin.HideOutMainMenuConfig.Value)
            {
                ___environmentUI_0.ShowEnvironment(false);

                GameObject FPSCamera = Plugin.FindRootObject("CommonUIScene", "FPS Camera");
                if (FPSCamera == null)
                {
                    FPSCamera = Plugin.FindRootObject("MenuUIScene", "FPS Camera");
                }
                if (FPSCamera == null)
                {
                    FPSCamera = Plugin.FindRootObject("DontDestroyOnLoad", "FPS Camera");
                }
                if (FPSCamera == null && !Plugin.initOnce)
                {
                    __instance.method_8(EMenuType.Hideout);
                    Plugin.initOnce = true;
                }
                if (FPSCamera != null)
                {
                    FPSCamera.gameObject.SetActive(true);

                    return;
                }
                Logger.LogError("FPS Camera Null");
                ___environmentUI_0.ShowEnvironment(true);

            }
            else
            {
                ___environmentUI_0.ShowEnvironment(true);
            }

            // Hide Group buttons on Taskbar
            if (Plugin.DisableGroupConfig.Value)
            {
                GameObject groupPannel = GameObject.Find("Preloader UI/Preloader UI/BottomPanel/Content/TaskBar/Tabs/GroupPanel/");
                if (groupPannel != null)
                {
                    groupPannel.gameObject.SetActive(false);
                }
            }

        }
    }

    //Add Map to Taskbar
    internal class MenuTaskBar_AwakePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(MenuTaskBar).GetMethod(nameof(MenuTaskBar.Awake));
        }

        [PatchPostfix]
        public static void Postfix(Dictionary<EMenuType, AnimatedToggle> ____toggleButtons, Dictionary<EMenuType,
                   HoverTooltipArea> ____hoverTooltipAreas, ref GameObject[] ____newInformation)
        {
            if (Plugin.mapOnTaskBarConfig.Value)
            {
                GameObject fleaMarketGameObject = GameObject.Find("Preloader UI/Preloader UI/BottomPanel/Content/TaskBar/Tabs/FleaMarket");
                if (fleaMarketGameObject != null)
                {
                    GameObject MapButtonGameObject = GameObject.Instantiate(fleaMarketGameObject);

                    MapButtonGameObject.name = "MAP Object";
                    MapButtonGameObject.transform.SetParent(fleaMarketGameObject.transform.parent, false);
                    MapButtonGameObject.transform.SetSiblingIndex(3);

                    GameObject MapButton = MapButtonGameObject.transform.GetChild(0).gameObject;
                    MapButton.name = "Map Button";

                    LocalizedText text = MapButtonGameObject.GetComponentInChildren<LocalizedText>();
                    if (text != null)
                    {
                        text.LocalizationKey = "";
                        text.method_2(Plugin.mapButtonTextConfig.Value);
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

                        AnimatedToggle animatedToggle = MapButtonGameObject.GetComponentInChildren<AnimatedToggle>();

                        //Bind MAP button action and SFX.
                        if (animatedToggle != null)
                        {

                            animatedToggle.onValueChanged.AddListener(async (arg) =>
                            {
                                try
                                {
                                    Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ButtonBottomBarClick);

                                    GameObject menuObj = GameObject.Find("Menu UI/UI/Matchmaker Location Selection/");
                                    if (menuObj == null)
                                    {
                                        Logger.LogError("Map Menu Null");
                                    }

                                    var sideSelectionMenuGO = GameObject.Find("Menu UI/UI/MatchMaker Side Selection Screen/");
                                    var sideSelectionInst = sideSelectionMenuGO.transform.GetComponent<MatchMakerSideSelectionScreen>();

                                    var watcher = menuObj.gameObject.AddComponent<Plugin.MenuWatcher>();
                                    watcher.OnMenuDisabled = () =>
                                    {
                                        Debug.Log("Menu was disabled, run my custom logic!");
                                        animatedToggle.Boolean_0 = false;
                                    };

                                    AccessTools.Field(typeof(MatchMakerSideSelectionScreen), "esideType_0").SetValue(sideSelectionInst, ESideType.Pmc);

                                    TarkovApplication.Exist(out TarkovApplication tarkovApp);
                                    if (tarkovApp != null)
                                    {
                                        var menuOperation = AccessTools.Field(tarkovApp.GetType(), "_menuOperation").GetValue(tarkovApp) as MainMenuControllerClass;
                                        if (menuOperation != null)
                                        {
                                            if (menuObj.activeInHierarchy)
                                            {
                                                menuOperation.ShowScreen(EMenuType.MainMenu, true);
                                                return;
                                            }

                                            menuOperation.ShowScreen(EMenuType.Play, true);
                                            sideSelectionInst.method_18();
                                        }
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
    }

    // 1. SCAV or PMC Selection Menu
    internal class SideSelection_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchMakerSideSelectionScreen), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(IHealthController), typeof(InventoryController) });
        }

        [PatchPostfix]
        static void Postfix(MatchMakerSideSelectionScreen __instance)
        {
            if (MenuLayoutChangesConfig.Value)
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
            else
            {
                __instance.transform.Find("CaptionsHolder").gameObject.SetActive(true);
                __instance.transform.Find("Description").gameObject.SetActive(true);
            }
        }
    }

    // 2. Level Select Map Menu
    internal class LocationSelection_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchMakerSelectionLocationScreen), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(MatchmakerPlayerControllerClass) });
        }

        [PatchPostfix]
        static void Postfix(MatchMakerSelectionLocationScreen __instance)
        {
            if (MenuLayoutChangesConfig.Value)
            {
                __instance.transform.Find("CaptionsHolder").gameObject.SetActive(false);

                var conditionsPannel = __instance.transform.Find("Conditions Panel");
                conditionsPannel.Find("Tiles").gameObject.SetActive(false);
                conditionsPannel.GetComponent<Image>().enabled = false;
                conditionsPannel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-440, 470);

                var locationInfo = __instance.transform.Find("Content").GetChild(0);
                locationInfo.GetComponent<RectTransform>().anchoredPosition = new Vector2(-800, -100);
                locationInfo.Find("Banner").GetComponent<RectTransform>().localScale = new Vector3(1.76f, 1.76f, 1);
                locationInfo.Find("DescriptionPanel").GetChild(0).GetComponent<CustomTextMeshProUGUI>().fontSize = 16;
                locationInfo.Find("DescriptionPanel").GetChild(1).GetChild(2).gameObject.SetActive(false);
                locationInfo.Find("DescriptionPanel").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);

                var map = __instance.transform.Find("Content").GetChild(1);
                map.GetComponent<RectTransform>().anchoredPosition = new Vector2(950, 100);
                map.GetComponent<RectTransform>().sizeDelta = new Vector2(796, 0);
            }
            else
            {
                __instance.transform.Find("CaptionsHolder").gameObject.SetActive(true);
            }
        }
    }

    // 3. Offline Raid Settings Menu
    internal class RaidSettingsScreen_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchmakerOfflineRaidScreen), "Show", new System.Type[] { typeof(InfoClass), typeof(RaidSettings), typeof(RaidSettings) });
        }

        [PatchPostfix]
        static void Postfix(MatchmakerOfflineRaidScreen __instance)
        {
            if (MenuLayoutChangesConfig.Value)
            {
                // Deactivates top text and spacers.
                __instance.transform.Find("Content").Find("Description").gameObject.SetActive(false);
                __instance.transform.Find("Content").Find("Space (1)").gameObject.SetActive(false);
                __instance.transform.Find("Content").Find("Space (2)").gameObject.SetActive(false);
            }
            else 
            {
                __instance.transform.Find("Content").Find("Description").gameObject.SetActive(true);
                __instance.transform.Find("Content").Find("Space (1)").gameObject.SetActive(true);
                __instance.transform.Find("Content").Find("Space (2)").gameObject.SetActive(true);
            }

            if (Plugin.SkipPreRaidMenusConfig.Value)
            {
                var nextButton = __instance.transform.Find("ScreenDefaultButtons/NextButton/");
                nextButton.GetComponent<DefaultUIButton>().method_11();
            }
        }
    }

    // 4. Insurance Menu
    internal class InsuranceScreen_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchmakerInsuranceScreen), "Show", new System.Type[] { typeof(IHealthController), typeof(InventoryController), typeof(ISession) });
        }

        [PatchPostfix]
        static void Postfix(MatchmakerInsuranceScreen __instance)
        {
            if (MenuLayoutChangesConfig.Value)
            {
                __instance.transform.Find("WarningPanel").gameObject.SetActive(false);
                __instance.transform.Find("Tab Bar").GetComponent<RectTransform>().anchoredPosition = new Vector2(1, -100);
                __instance.transform.Find("ItemsToInsurePanel").GetComponent<RectTransform>().sizeDelta = new Vector2(5, -305);
                __instance.transform.Find("ItemsToInsurePanel").GetComponent<RectTransform>().anchoredPosition = new Vector2(-3.5f, 20);
                __instance.transform.Find("ItemsToInsurePanel").Find("ItemsToInsureList").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -80);
                __instance.transform.Find("ItemsToInsurePanel").Find("ItemsToInsureList").GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 490);
            }

            if (Plugin.SkipPreRaidMenusConfig.Value)
            {
                var nextButton = __instance.transform.Find("ScreenDefaultButtons/NextButton/");
                nextButton.GetComponent<DefaultUIButton>().method_11();
            }

            if (Plugin.HideOutMainMenuConfig.Value)
            {
                GameObject fpsCAM = Plugin.FindFPSCam();
                if (fpsCAM != null)
                {
                    fpsCAM.SetActive(true);
                    return;
                }
                return;
            }
        }
    }

    // 5. Accept Location Menu
    internal class AcceptLocationPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchMakerAcceptScreen), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(RaidSettings) });
        }

        [PatchPostfix]
        static void Postfix(MatchMakerAcceptScreen __instance)
        {
            PreloaderUI preloaderUI = MonoBehaviourSingleton<PreloaderUI>.Instance;
            preloaderUI.SetMenuTaskBarVisibility(true);

            if (SkipPreRaidMenusConfig.Value)
            {
                var taskbar = preloaderUI.transform.Find("Preloader UI/BottomPanel/Content/TaskBar/").GetComponent<MenuTaskBar>();
                if (taskbar != null)
                {
                    taskbar.SetButtonsInteractable(true);
                }
                var backButton = __instance.transform.Find("ScreenDefaultButtons");
                backButton.gameObject.SetActive(false);
            }

            if(MenuLayoutChangesConfig.Value) 
            {
                __instance.transform.Find("CaptionsHolder").gameObject.SetActive(false);
                var previewPanel = __instance.transform.Find("PreviewsPanel");
                previewPanel.transform.Find("CurrentPlayerModelView").Find("PlayerMVObject").Find("Camera_acceptScreen").GetComponent<Transform>().localEulerAngles = new Vector3(358f, 352.3f, 0f);
                previewPanel.transform.Find("CurrentPlayerModelView").Find("PlayerMVObject").Find("Camera_acceptScreen").GetComponent<Transform>().localPosition = new Vector3(0.81f, -0.08f, -1.8f);
            }

            if (Plugin.HideOutMainMenuConfig.Value)
            {
                GameObject fpsCAM = Plugin.FindFPSCam();
                if (fpsCAM != null)
                {
                    fpsCAM.SetActive(true);
                    return;
                }
                return;
            }
        }
    }

    // 6. Raid Loading Screen
    internal class TimeHasCome_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchmakerTimeHasCome), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(MatchmakerPlayerControllerClass) });
        }

        [PatchPostfix]
        static void Postfix(MatchmakerBannersPanel ____bannersPanel, MatchmakerTimeHasCome __instance)
        {
            Plugin.initOnce = false;
            PreloaderUI preloaderUI = MonoBehaviourSingleton<PreloaderUI>.Instance;

            var locationNamePanel = __instance.transform.Find("Location Name Panel");
            var locationNameGUI = locationNamePanel.gameObject.transform.Find("Name").GetComponent<CustomTextMeshProUGUI>();
            var accentColor = Plugin.GetMapColorConfig(locationNameGUI.text);

            if (MenuLayoutChangesConfig.Value)
            {
                preloaderUI.SetMenuTaskBarVisibility(false);


                //Deactivate banner, logo, tips, and loading spinner.
                var bannerPanel = ____bannersPanel.gameObject;
                if (bannerPanel != null)
                {
                    bannerPanel.gameObject.SetActive(false);
                }

                // Hide Header Text
                var captionsHolder = __instance.transform.Find("CaptionsHolder");
                if (captionsHolder != null)
                {
                    captionsHolder.gameObject.SetActive(false);
                }

                //Hide EFT Logo
                var logo = __instance.transform.Find("Logo");
                if (logo != null)
                {
                    logo.gameObject.SetActive(false);
                }

                //Hide Loading Spinner
                var loader = __instance.transform.Find("Loader");
                if (loader != null)
                {
                    loader.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(925, 30);
                    //loader.gameObject.SetActive(false);
                }

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

                    locationNameGUI.color = accentColor.Value;

                    locationNamePanel.gameObject.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(450, 150);
                    locationNamePanel.gameObject.transform.Find("Background").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

                    Plugin.GetMapColorConfig(locationNameGUI.text).SettingChanged += (s, e) =>
                    {
                        locationNameGUI.color = Plugin.GetMapColorConfig(locationNameGUI.text).Value;
                    };

                    if (ChangeUISceneOnLoading.Value)
                    {
                        setLoadRaidBackground(locationNameGUI.text);
                    }
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

                        var mainLight = playerModellights.Find("Main Light");
                        mainLight.GetComponent<Transform>().localEulerAngles = new Vector3(60f, 60f, 20f);
                        mainLight.GetComponent<Transform>().localPosition = new Vector3(-0.243f, -294.1938f, 4.65f);
                        mainLight.GetComponent<Light>().type = LightType.Spot;
                        mainLight.GetComponent<Light>().spotAngle = 65;
                        mainLight.GetComponent<Light>().range = 1.3f;
                        mainLight.GetComponent<Light>().intensity = 6f;
                        mainLight.GetComponent<Light>().color = new Color(1, 0.86f, 0.74f, 1);

                        var fillLight = playerModellights.Find("Fill Light");
                        fillLight.GetComponent<Transform>().localEulerAngles = new Vector3(0f, 130f, 200f);
                        //fillLight.GetComponent<Light>().intensity = 0.3;
                        fillLight.GetComponent<Light>().color = accentColor.Value;

                        Plugin.GetMapColorConfig(locationNameGUI.text).SettingChanged += (s, e) =>
                        {
                            fillLight.GetComponent<Light>().color = Plugin.GetMapColorConfig(locationNameGUI.text).Value;
                        };
                    }
                }

                if (Plugin.HideOutMainMenuConfig.Value)
                {
                    ShowEnvironmentUI(true);
                    ToggleEnvironmentBackground(true);
                }
            }
        }
    }

    // 7."Deploying In.." Screen
    internal class FinalCountdown_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchmakerFinalCountdown), "Show", new System.Type[] { typeof(Profile), typeof(DateTime) });
        }

        [PatchPostfix]
        static void Postfix(MatchmakerFinalCountdown __instance)
        {
            if (MenuLayoutChangesConfig.Value)
            {
                __instance.transform.Find("Logo").gameObject.SetActive(false);
            }
            if (HideMenuBackgroundInRaid.Value)
            {
                EnvironmentUI environmentUI = MonoBehaviourSingleton<EnvironmentUI>.Instance;
                environmentUI.gameObject.SetActive(false);
            }
        }
    }

    //HUD Edits
    internal class QuickSlotsHUD_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(InventoryScreenQuickAccessPanel), "Show", new System.Type[] { typeof(InventoryController), typeof(ItemUiContext), typeof(GamePlayerOwner), typeof(InsuranceCompanyClass) });
        }

        [PatchPostfix]
        static void Postfix(InventoryScreenQuickAccessPanel __instance)
        {
            // Hide Quickslot HUD
            if (Plugin.HideQuickSlotsConfig.Value)
            {
                GameObject QuickSlotObject = GameObject.Find("Common UI/Common UI/EFTBattleUIScreen Variant/QuickAccessPanel/");
                if (QuickSlotObject != null)
                {
                    QuickSlotObject.transform.GetChild(0).gameObject.SetActive(false);
                    QuickSlotObject.transform.GetChild(1).gameObject.SetActive(false);
                }

                Plugin.HideQuickSlotsConfig.SettingChanged += (s, e) =>
                {
                    if (QuickSlotObject.transform.GetChild(0).gameObject.activeInHierarchy)
                    {
                        QuickSlotObject.transform.GetChild(0).gameObject.SetActive(false);
                        QuickSlotObject.transform.GetChild(1).gameObject.SetActive(false);
                    }
                    else
                    {
                        QuickSlotObject.transform.GetChild(0).gameObject.SetActive(true);
                        QuickSlotObject.transform.GetChild(1).gameObject.SetActive(true);
                    }
                };
            }

            // Hide Character Stance HUD
            if (Plugin.StanceSillhouetteConfig.Value)
            {
                GameObject BattleStanceObject = GameObject.Find("Common UI/Common UI/EFTBattleUIScreen Variant/BattleStancePanel/");
                if (BattleStanceObject != null)
                {
                    BattleStanceObject.transform.GetChild(1).gameObject.SetActive(false);
                    BattleStanceObject.transform.GetChild(3).gameObject.SetActive(false);
                    BattleStanceObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                }

                Plugin.StanceSillhouetteConfig.SettingChanged += (s, e) =>
                {
                    if (BattleStanceObject.transform.GetChild(1).gameObject.activeInHierarchy)
                    {
                        BattleStanceObject.transform.GetChild(1).gameObject.SetActive(false);
                        BattleStanceObject.transform.GetChild(3).gameObject.SetActive(false);
                        BattleStanceObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                    }
                    else
                    {
                        BattleStanceObject.transform.GetChild(1).gameObject.SetActive(true);
                        BattleStanceObject.transform.GetChild(3).gameObject.SetActive(true);
                        BattleStanceObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                    }
                };
            }
        }
    }

    //Auto selects/focuses a hideout area on first load
    internal class HideoutOverlay_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(HideoutScreenOverlay), "Show", new System.Type[] { typeof(HideoutPlayerOwner), typeof(bool), typeof(ISession), typeof(AreaData[]), typeof(HideoutScreenRear) });
        }

        [PatchPostfix]
        static void Postfix(HideoutScreenOverlay __instance)
        {
            int areatoFocus = 2;

            if (!Plugin.initOnce)
            {
                Plugin.focusHideoutArea(__instance, areatoFocus);
            }
            Plugin.initOnce = true;
        }
    }

    //Reactivates Envirmont UI when leaving Raid
    internal class SessionResultExitStatus_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(SessionResultExitStatus), "Show", new System.Type[] { typeof(Profile), typeof(GClass1952), typeof(ESideType), typeof(ExitStatus), typeof(TimeSpan), typeof(ISession), typeof(bool) });
        }

        [PatchPostfix]
        static void Postfix(SessionResultExperienceCount __instance)
        {
            EnvironmentUI environmentUI = MonoBehaviourSingleton<EnvironmentUI>.Instance;
            environmentUI.gameObject.SetActive(true);
        }
    }

    //Clock Patch
    internal class InventoryScreen_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(InventoryScreen), "Show", new System.Type[] { typeof(IHealthController), typeof(InventoryController), typeof(AbstractQuestControllerClass), typeof(AbstractAchievementControllerClass), typeof(GClass3695), typeof(CompoundItem), typeof(EInventoryTab), typeof(ISession), typeof(ItemContextAbstractClass), typeof(Boolean) });
        }

        [PatchPostfix]
        public static void Postfix(InventoryScreen __instance, ISession ___iSession)
        {
            // Add Clock to Inventory Screen
            if (Plugin.EnableClockPatchConfig.Value)
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
        }
    }
    
    //Hideout Area Pannel Edits
    internal class AreaScreenSubstrate_AwakePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(AreaScreenSubstrate), "Awake");
        }

        [PatchPostfix]
        static void Postfix(AreaScreenSubstrate __instance)
        {
            __instance.transform.Find("Fader").gameObject.SetActive(false);
            __instance.transform.Find("Border").gameObject.SetActive(false);
            __instance.transform.Find("CaptionPanel").GetComponent<Image>().enabled = false;
            __instance.transform.Find("Content/NextLevel/BottomPanel/").GetComponent<Image>().enabled = false;
            __instance.transform.Find("Content/CurrentLevel/BottomPanel/").GetComponent<Image>().enabled = false;
        }
    }
}