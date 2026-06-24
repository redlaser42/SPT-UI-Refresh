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
                Utils.focusHideoutArea(__instance, areatoFocus);
            }
            Plugin.initOnce = true;
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