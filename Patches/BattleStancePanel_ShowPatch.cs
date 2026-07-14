using EFT;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace UIRefresh.Patches
{
    internal class BattleStancePanel_ShowPatch : ModulePatch
    {
        public static GameObject? BattleStanceObject = null;

        public static GameObject? RaidWarningTimerObject = null;


        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(BattleStancePanel), "Show");
        }

        [PatchPostfix]
        static void Postfix(BattleStancePanel __instance,Player player)
        {
            BattleStanceObject = GameObject.Find("Common UI/Common UI/EFTBattleUIScreen Variant/BattleStancePanel/Stances/");

            if (BattleStanceObject != null)
            {
                StanceSillhouetteUpdate();
            }
        }

        public static void StanceSillhouetteUpdate()
        {
            if (BattleStanceObject != null)
            {
                BattleStanceObject.SetActive(!Plugin.Instance.UIRefreshConfig.HideStanceSillhouette.Value);
            }
        }
    }
}