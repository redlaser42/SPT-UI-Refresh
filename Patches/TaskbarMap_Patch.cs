using Comfort.Common;
using EFT;
using EFT.UI;
using EFT.UI.Matchmaker;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;



namespace UIRefresh.Patches
{
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
                                        var menuOperation = AccessTools.Field(tarkovApp.GetType(), "mainMenuControllerClass").GetValue(tarkovApp) as MainMenuControllerClass;
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

}