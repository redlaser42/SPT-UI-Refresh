using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UIRefresh.Patches
{
    //Hides the big orange banner in the main menu.
    internal class HideAlphaWarning_Patch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MenuScreen), nameof(MenuScreen.method_3));
        }

        [PatchPostfix]
        static void Postfix(MenuScreen __instance, GameObject ____alphaWarningGameObject)
        {
            ____alphaWarningGameObject.SetActive(false);
        }
    }
}