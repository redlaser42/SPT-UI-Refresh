using EFT;
using EFT.InventoryLogic;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UIRefresh.Patches
{
    internal class QuickSlotsHUD_ShowPatch : ModulePatch
    {
        public static Image stand0 = null;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(InventoryScreenQuickAccessPanel), "Show", new System.Type[] { typeof(InventoryController), typeof(ItemUiContext), typeof(GamePlayerOwner), typeof(InsuranceCompanyClass) });
        }

        [PatchPostfix]
        static void Postfix(InventoryScreenQuickAccessPanel __instance)
        {

            GameObject BattleStanceObject = GameObject.Find("Common UI/Common UI/EFTBattleUIScreen Variant/BattleStancePanel/Stances/Stand0");
            if (BattleStanceObject != null)
            {
                stand0 = BattleStanceObject.GetComponent<Image>();

                StanceSillhouetteUpdate();
            }
        }
        public static void StanceSillhouetteUpdate()
        {
            stand0.enabled = Plugin.HideStanceSillhouette.Value;
        }
     }
}