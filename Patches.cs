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
}