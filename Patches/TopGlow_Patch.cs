using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine.UI;

namespace UIRefresh.Patches
{
    internal class HideTopGlowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(EnvironmentUI), nameof(EnvironmentUI.method_0));
        }

        [PatchPostfix]
        static void Postfix(Image imageToFadeOut, Image imageToFadeIn, bool forced)
        {
            imageToFadeIn.gameObject.SetActive(false);
        }
    }
}