using Comfort.Common;
using EFT;
using EFT.UI;
using EFT.UI.Matchmaker;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;



namespace UIRefresh.Patches
{
    //Add Map to Taskbar
    //This code is from the FIKA menu taskbar patch that adds the Download Profile Button. 

    internal class MenuTaskBar_AwakePatch : ModulePatch
    {
        public static LocalizedText? mapButtonText;

        public static GameObject? MapButtonGameObject;
        protected override MethodBase GetTargetMethod()
        {
            return typeof(MenuTaskBar).GetMethod(nameof(MenuTaskBar.Awake));
        }

        [PatchPostfix]
        public static void Postfix(Dictionary<EMenuType, AnimatedToggle> ____toggleButtons, Dictionary<EMenuType,
                   HoverTooltipArea> ____hoverTooltipAreas, ref GameObject[] ____newInformation)
        {
            GameObject fleaMarketGameObject = GameObject.Find("Preloader UI/Preloader UI/BottomPanel/Content/TaskBar/Tabs/FleaMarket");
            MapButtonGameObject = GameObject.Instantiate(fleaMarketGameObject);

                if (fleaMarketGameObject != null && Plugin.Instance.UIRefreshConfig.mapOnTaskBarConfig.Value)
                {
                    var toggle = MapButtonGameObject.GetComponentInChildren<AnimatedToggle>();

                    Logger.LogInfo("Event Count: "+toggle.onValueChanged.GetPersistentEventCount());
                    
                    MapButtonGameObject.name = "MAP Object";
                    MapButtonGameObject.transform.SetParent(fleaMarketGameObject.transform.parent, false);
                    MapButtonGameObject.transform.SetSiblingIndex(3);

                    GameObject MapButton = MapButtonGameObject.transform.GetChild(0).gameObject;
                    MapButton.name = "Map Button";

                    mapButtonText = MapButtonGameObject.GetComponentInChildren<LocalizedText>();
                    mapButtonText.LocalizationKey = "";
                    UpdateMapButtonText();

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
                                    if (menuObj != null)
                                    {
                                        var watcher = menuObj.GetComponent<Utils.MenuWatcher>();

                                        if (watcher == null)
                                        {
                                            watcher = menuObj.AddComponent<Utils.MenuWatcher>();
                                        }

                                        watcher.OnMenuDisabled = () =>
                                        {
                                            animatedToggle.ToggleSilent(false);
                                        };
                                    }

                                    var sideSelectionInst = GameObject.Find("Menu UI/UI/MatchMaker Side Selection Screen/").GetComponent<MatchMakerSideSelectionScreen>();

                                    AccessTools.Field(typeof(MatchMakerSideSelectionScreen), "esideType_0").SetValue(sideSelectionInst, ESideType.Pmc);

                                    TarkovApplication.Exist(out TarkovApplication tarkovApp);
                                    if (tarkovApp != null)
                                    {
                                        var menuOperation = AccessTools.Field(tarkovApp.GetType(), "mainMenuControllerClass").GetValue(tarkovApp) as MainMenuControllerClass;
                                        if (menuOperation != null)
                                        {
                                            if (menuObj.activeInHierarchy)
                                            {
                                                menuOperation.ShowScreen(EMenuType.MainMenu, true);
                                                return;
                                            }

                                            menuOperation.ShowScreen(EMenuType.Play, true);
                                            if( sideSelectionInst != null)
                                            {
                                                sideSelectionInst.method_18();
                                            }
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
        
        public static void UpdateMapButtonText()
        {
            if (mapButtonText == null)
            {
                return;
            }
            mapButtonText.method_2(Plugin.Instance.UIRefreshConfig.mapButtonTextConfig.Value);
        }
        public static void UpdateMapButtonSelectability()
        {

        }
    }
}