using EFT;
using EFT.UI;
using EFT.UI.Gestures;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;
using UIRefresh.Config;

namespace UIRefresh.Patches
{
    internal class GesturesQuickPanel_ShowPatch : ModulePatch
    {
        public static GameObject? GesturesQuickPanelObject = null;
        public static CanvasGroup? GesturesQuickPanelCanvasGroup = null;

        protected override MethodBase GetTargetMethod()
        {
            return typeof(GesturesQuickPanel).GetMethod(nameof(GesturesQuickPanel.Show));
        }

        [PatchPostfix]
        static void Postfix(GesturesQuickPanel __instance, CanvasGroup ____canvasGroup)
        {
            GesturesQuickPanelObject = __instance.gameObject;
            GesturesQuickPanelCanvasGroup = ____canvasGroup;

            UpdateGesturesQuickPanel();
        }

        public static void UpdateGesturesQuickPanel()
        {
            if (GesturesQuickPanelCanvasGroup != null)
            {
                GesturesQuickPanelCanvasGroup.alpha = Plugin.Instance.UIRefreshConfig.GesturesQuickPanelAlpha.Value;
            }
        }
    }
}