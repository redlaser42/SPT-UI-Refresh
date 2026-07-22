using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace UIRefresh.Patches
{
    internal class HideGroupPanel_Patch : ModulePatch
    {
        public static GameObject? groupPanel;
        
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GroupPanel), "Show", new System.Type[] { typeof(MatchmakerPlayerControllerClass), typeof(SocialNetworkClass)});
        }

        [PatchPostfix]
        static void Postfix(MenuScreen __instance)
        {
            groupPanel = __instance.gameObject;
            UpdateGroupPanel();
        }
        public static void UpdateGroupPanel()
        {
            if (groupPanel != null)
            {
                groupPanel.SetActive(!Plugin.Instance.UIRefreshConfig.DisableGroupConfig.Value);
            }
        }
    }
}