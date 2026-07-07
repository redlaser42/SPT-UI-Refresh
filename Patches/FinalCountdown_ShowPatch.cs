using EFT;
using EFT.UI;
using EFT.UI.Matchmaker;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UIRefresh.Config;

namespace UIRefresh.Patches
{
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
            if (Plugin.Instance.UIRefreshConfig.MenuLayoutChangesConfig.Value)
            {
                __instance.transform.Find("Logo").gameObject.SetActive(false);
            }
            if (Plugin.Instance.UIRefreshConfig.HideMenuBackgroundInRaid.Value)
            {
                EnvironmentUI environmentUI = MonoBehaviourSingleton<EnvironmentUI>.Instance;
                environmentUI.gameObject.SetActive(false);
            }
        }
    }
}