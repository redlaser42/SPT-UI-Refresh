using EFT;
using EFT.UI;
using Fika.Core.UI.Custom;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


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
            if (Plugin.HideOutMainMenuConfig.Value)
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
				if (FPSCamera == null && !Plugin.initOnce)
				{
					__instance.method_8(EMenuType.Hideout);
					Plugin.initOnce = true;
				}
				if (FPSCamera != null)
				{
					FPSCamera.gameObject.SetActive(true);

					return;
				}
				Logger.LogError("FPS Camera Null");
				___environmentUI_0.ShowEnvironment(true);

			}
			else
			{
				___environmentUI_0.ShowEnvironment(true);
			}

            // Hide Group buttons on Taskbar
            if (Plugin.DisableGroupConfig.Value)
			{
				GameObject groupPannel = GameObject.Find("Preloader UI/Preloader UI/BottomPanel/Content/TaskBar/Tabs/GroupPanel/");
				if (groupPannel != null)
				{
					groupPannel.gameObject.SetActive(false);
				}
			}
        }
	}
}