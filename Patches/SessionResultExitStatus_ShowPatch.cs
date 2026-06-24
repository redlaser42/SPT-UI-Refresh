using EFT;
using EFT.UI;
using EFT.UI.SessionEnd;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UnityEngine.UI;

namespace UIRefresh.Patches
{

    //Reactivates Envirmont UI when leaving Raid
    internal class SessionResultExitStatus_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(SessionResultExitStatus), "Show", new System.Type[] { typeof(Profile), typeof(LastPlayerStateClass), typeof(ESideType), typeof(ExitStatus), typeof(TimeSpan), typeof(ISession), typeof(bool) });
        }

        [PatchPostfix]
        static void Postfix(SessionResultExperienceCount __instance)
        {
            EnvironmentUI environmentUI = MonoBehaviourSingleton<EnvironmentUI>.Instance;
            environmentUI.gameObject.SetActive(true);
        }
    }
}