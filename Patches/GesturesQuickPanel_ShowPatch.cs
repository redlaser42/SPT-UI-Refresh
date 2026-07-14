using EFT;
using EFT.UI;
using EFT.UI.Gestures;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace UIRefresh.Patches
{
    internal class GesturesQuickPanel_ShowPatch : ModulePatch
    {
        public static GameObject? GesturesQuickPanelObject = null;

        protected override MethodBase GetTargetMethod()
        {
            return typeof(GesturesQuickPanel).GetMethod(nameof(GesturesQuickPanel.Show));
        }

        [PatchPostfix]
        static void Postfix(GesturesQuickPanel __instance)
        {
            Logger.LogError("Fired Hide Context Prompt");

            GesturesQuickPanelObject = __instance.gameObject;

            if (GesturesQuickPanelObject != null)
            {
                HideGesturesQuickPanelUpdate();

            }
        }

        public static void HideGesturesQuickPanelUpdate()
        {
            if (GesturesQuickPanelObject != null)
            {
                GesturesQuickPanelObject.SetActive(!Plugin.Instance.UIRefreshConfig.HideGesturesQuickPanel.Value);
            }
        }
    }
}