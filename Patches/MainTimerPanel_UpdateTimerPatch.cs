using EFT.UI.BattleTimer;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UnityEngine;

namespace UIRefresh.Patches
{
    internal class MainTimerPanel_UpdatePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(MainTimerPanel).GetMethod(nameof(MainTimerPanel.UpdateTimer));
        }

        [PatchPrefix]
        static bool Prefix(MainTimerPanel __instance, Color ____warningColor, TimeSpan ___TimeSpan)
        {
            if (Plugin.Instance.UIRefreshConfig.HideRaidTimerWarning.Value)
            {
                TimerPanelUpdateReversePatch.UpdateTimer(__instance);

                __instance.TimerText.color = new Color(0,0,1);

                bool lowOnTime = ___TimeSpan.TotalSeconds < 600.0;

                if (lowOnTime && !__instance.ForcePull)
                {
                    __instance.DisplayTimer();
                    if (__instance.TimerText.color != ____warningColor)
                    {
                        __instance.TimerText.color = ____warningColor;
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}