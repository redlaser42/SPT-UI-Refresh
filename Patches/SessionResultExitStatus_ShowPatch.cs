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
            return AccessTools.Method(typeof(SessionEndUI), "Awake");
        }

        [PatchPostfix]
        static void Postfix(SessionEndUI __instance)
        {
            EnvironmentUI environmentUI = MonoBehaviourSingleton<EnvironmentUI>.Instance;
            environmentUI.gameObject.SetActive(true);
        }
    }
}