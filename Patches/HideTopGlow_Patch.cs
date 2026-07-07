using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UIRefresh.Config;
using UnityEngine;
using UnityEngine.UI;

namespace UIRefresh.Patches
{
    internal class HideTopGlowPatch : ModulePatch
    {
        public static Image? topGlow = null;
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(EnvironmentUI), nameof(EnvironmentUI.method_0));
        }

        [PatchPostfix]
        static void Postfix(Image imageToFadeOut, Image imageToFadeIn, bool forced)
        {
            topGlow = imageToFadeIn;
            UpdateTopGlow();
        }

        public static void UpdateTopGlow()
        {
            topGlow.enabled = !Plugin.Instance.UIRefreshConfig.HideTopGlow.Value;
        }
    }
}