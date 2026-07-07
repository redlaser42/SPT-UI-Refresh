using EFT;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UIRefresh.Config;
using UnityEngine;
using UnityEngine.UI;

namespace UIRefresh.Patches
{
    internal class HideGroupPanel_Patch : ModulePatch
    {
        public static GameObject groupPannel = null;
        
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GroupPanel), "Show", new System.Type[] { typeof(MatchmakerPlayerControllerClass), typeof(SocialNetworkClass)});
        }

        [PatchPostfix]
        static void Postfix(MenuScreen __instance)
        {
            groupPannel = __instance.gameObject;
            UpdateGroupPanel();
        }
        public static void UpdateGroupPanel()
        {
            groupPannel.SetActive(!Plugin.Instance.UIRefreshConfig.DisableGroupConfig.Value);
        }
    }
}