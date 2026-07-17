using EFT;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;


namespace UIRefresh.Patches
{
	//Main Menu Show Patch. Hides groups and other things
	internal class MenuScreen_ShowPatch : ModulePatch
	{
		protected override MethodBase GetTargetMethod()
		{
			return AccessTools.Method(typeof(MenuScreen), "Show", new System.Type[] { typeof(Profile), typeof(MatchmakerPlayerControllerClass), typeof(ESessionMode) });
		}

		[PatchPostfix]
		static void Postfix(MenuScreen __instance, EnvironmentUI ___environmentUI_0)
		{
            // Show Hideout in Main Menu
            if (Plugin.Instance.UIRefreshConfig.HideOutMainMenuConfig.Value)
			{
				___environmentUI_0.ShowEnvironment(false);

				GameObject FPSCamera = Utils.FindRootObject("CommonUIScene", "FPS Camera");
				if (FPSCamera == null)
				{
					FPSCamera = Utils.FindRootObject("MenuUIScene", "FPS Camera");
				}
				if (FPSCamera == null)
				{
					FPSCamera = Utils.FindRootObject("DontDestroyOnLoad", "FPS Camera");
				}
				if (FPSCamera == null)
				{
					__instance.method_8(EMenuType.Hideout);
				}
				if (FPSCamera != null)
				{
					FPSCamera.gameObject.SetActive(true);
                    return;
				}
				___environmentUI_0.ShowEnvironment(true);

			}
			else
			{
				___environmentUI_0.ShowEnvironment(true);
			}
        }
	}
}