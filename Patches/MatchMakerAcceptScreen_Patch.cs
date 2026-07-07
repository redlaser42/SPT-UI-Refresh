using EFT;
using EFT.UI;
using EFT.UI.Matchmaker;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UIRefresh.Config;
using UnityEngine;

namespace UIRefresh.Patches
{
    // 5. Accept Location Menu
    internal class MatchMakerAcceptScreen_Patch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchMakerAcceptScreen), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(RaidSettings) });
        }

        [PatchPostfix]
        static void Postfix(MatchMakerAcceptScreen __instance)
        {
            PreloaderUI preloaderUI = MonoBehaviourSingleton<PreloaderUI>.Instance;
            preloaderUI.SetMenuTaskBarVisibility(true);

            if (Plugin.Instance.UIRefreshConfig.SkipPreRaidMenusConfig.Value)
            {
                var taskbar = preloaderUI.transform.Find("Preloader UI/BottomPanel/Content/TaskBar/").GetComponent<MenuTaskBar>();
                if (taskbar != null)
                {
                    taskbar.SetButtonsInteractable(true);
                }
                var backButton = __instance.transform.Find("ScreenDefaultButtons");
                backButton.gameObject.SetActive(false);
            }

            if (Plugin.Instance.UIRefreshConfig.MenuLayoutChangesConfig.Value)
            {
                __instance.transform.Find("CaptionsHolder").gameObject.SetActive(false);
                var previewPanel = __instance.transform.Find("PreviewsPanel");
                previewPanel.transform.Find("CurrentPlayerModelView").Find("PlayerMVObject").Find("Camera_acceptScreen").GetComponent<Transform>().localEulerAngles = new Vector3(358f, 352.3f, 0f);
                previewPanel.transform.Find("CurrentPlayerModelView").Find("PlayerMVObject").Find("Camera_acceptScreen").GetComponent<Transform>().localPosition = new Vector3(0.81f, -0.08f, -1.8f);
            }

            if (Plugin.Instance.UIRefreshConfig.HideOutMainMenuConfig.Value)
            {
                GameObject fpsCAM = Utils.FindFPSCam();
                if (fpsCAM != null)
                {
                    fpsCAM.SetActive(true);
                    return;
                }
                return;
            }
        }
    }
}