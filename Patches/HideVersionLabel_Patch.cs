using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UIRefresh.Patches
{
    //Hides version number in the bottom left. 
    internal class HideVersionLabelPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(PreloaderUI), nameof(PreloaderUI.method_6));
        }

        [PatchPostfix]
        static void Postfix(PreloaderUI __instance, LocalizedText ____alphaVersionLabel)
        {
            ____alphaVersionLabel.gameObject.SetActive(false);
        }
    }
}