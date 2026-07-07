using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UIRefresh.Config;
using UnityEngine;

namespace UIRefresh.Patches
{
    //Hides the PVP to PVE toggle button in the bottom right. 
    internal class HidePVEButton_Patch : ModulePatch
    {
        public static GameObject gamemodeButton = null;
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MenuScreen), nameof(MenuScreen.method_3));
        }

        [PatchPostfix]
        static void Postfix(MenuScreen __instance, ChangeGameModeButton ____toggleGameModeButton)
        {
            gamemodeButton = ____toggleGameModeButton.gameObject;
            UpdateGameModeButton();
        }
        public static void UpdateGameModeButton()
        {
            gamemodeButton.SetActive(!Plugin.Instance.UIRefreshConfig.HidePVEButton.Value);
        }
    }
}