using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UIRefresh.Config;
using UnityEngine;
using UnityEngine.UI;

namespace UIRefresh.Patches
{
    //Hides the big orange banner in the main menu.
    internal class HideBetaBanner_Patch : ModulePatch
    {
        public static GameObject? warningBanner = null;
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MenuScreen), nameof(MenuScreen.method_3));
        }

        [PatchPostfix]
        static void Postfix(MenuScreen __instance, GameObject ____alphaWarningGameObject)
        {
            warningBanner = ____alphaWarningGameObject;
            UpdateBetaBanner();
        }
        public static void UpdateBetaBanner()
        {
            warningBanner.SetActive(!Plugin.Instance.UIRefreshConfig.HideBetaBanner.Value);
        }
    }
}