using EFT.HealthSystem;
using EFT.InventoryLogic;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UnityEngine;

namespace UIRefresh.Patches
{ //Clock Patch
    internal class InventoryScreen_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(InventoryScreen), "Show", new System.Type[] { typeof(IHealthController), typeof(InventoryController), typeof(AbstractQuestControllerClass), typeof(AbstractAchievementControllerClass), typeof(AbstractPrestigeControllerClass), typeof(CompoundItem), typeof(EInventoryTab), typeof(ISession), typeof(ItemContextAbstractClass), typeof(Boolean) });
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
                    clockText.text = Utils.GetRaidTime(___iSession);
                    return;
                }
                //Create for first time
                var clockWidget = new GameObject("Clock Widget");
                clockWidget.transform.SetParent(clockParent, false);
                var newClockText = clockWidget.AddComponent<TMPro.TextMeshProUGUI>();
                newClockText.text = Utils.GetRaidTime(___iSession);
                newClockText.fontSize = 38;
                clockWidget.GetComponent<RectTransform>().anchoredPosition = new Vector2(170, -395);
            }
        }
    }
}