using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UIRefresh.Patches
{
    //Hides the PVP to PVE toggle button in the bottom right. 
    internal class HideGameModePatch : ModulePatch
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
            gamemodeButton.SetActive(Plugin.HideGameModeButton.Value);
        }
    }
}