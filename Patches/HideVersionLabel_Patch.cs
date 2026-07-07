using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace UIRefresh.Patches
{
    //Hides version number in the bottom left. 
    internal class HideVersionLabelPatch : ModulePatch
    {
        public static GameObject? versionLabel = null;
        
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(PreloaderUI), nameof(PreloaderUI.method_6));
        }

        [PatchPostfix]
        static void Postfix(PreloaderUI __instance, LocalizedText ____alphaVersionLabel)
        {
            versionLabel = ____alphaVersionLabel.gameObject;
            UpdateVersionLabel();
        }
        public static void UpdateVersionLabel()
        {
            if(Plugin.Instance.UIRefreshConfig.VersionLabelVisability != null)
            {
                versionLabel.SetActive(!Plugin.Instance.UIRefreshConfig.VersionLabelVisability.Value);
                return;
            }
            Logger.LogError("Plugin Instance UI Refresh Config Null");
        }
    }
}