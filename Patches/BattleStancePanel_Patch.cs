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
    internal class BattleStancePanel_ShowPatch : ModulePatch
    {
        public static GameObject BattleStanceObject = null;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(BattleStancePanel), "Show");
        }

        [PatchPostfix]
        static void Postfix(BattleStancePanel __instance,Player player)
        {
            Logger.LogError("PostFix fired");
            BattleStanceObject = GameObject.Find("Common UI/Common UI/EFTBattleUIScreen Variant/BattleStancePanel/Stances/");
            if (BattleStanceObject != null)
            {
                Logger.LogError("Fired Stance");
                StanceSillhouetteUpdate();
            }
            else
            {
                Logger.LogError("Battlestance Obj Null Post Fix");
            }
        }
        public static void StanceSillhouetteUpdate()
        {
            if (BattleStanceObject != null)
            {
                BattleStanceObject.SetActive(Plugin.ShowStanceSillhouette.Value);
            }
            else
            {
               Logger.LogError("Battlestance Obj Null");
            }
        }
     }
}