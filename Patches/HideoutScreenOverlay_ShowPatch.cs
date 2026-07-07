using EFT;
using EFT.Hideout;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UIRefresh.Config;
using UnityEngine;

namespace UIRefresh.Patches
{
    //Hides the PVP to PVE toggle button in the bottom right. 
    internal class HideoutScreenOverlay_Show : ModulePatch
    {
        static AreaData? autoSelectedArea { get; set; }

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(HideoutScreenOverlay), nameof(HideoutScreenOverlay.Show));
        }

        [PatchPostfix]
        static void Postfix(HideoutScreenOverlay __instance)

        {
            autoSelectedArea = __instance.transform.Find("BottomAreasPanel/Scroll View/Viewport/Content/").GetChild(0).GetComponent<AreaPanel>().Data;
            if (autoSelectedArea != null)
            {
                Logger.LogError(autoSelectedArea.ToString());
                __instance.transform.Find("BottomAreasPanel").GetComponent<AreasPanel>().method_6();
                TarkovApplication.Exist(out TarkovApplication tarkovApp);
                if (tarkovApp != null)
                {
                    var menuOperation = AccessTools.Field(tarkovApp.GetType(), "mainMenuControllerClass").GetValue(tarkovApp) as MainMenuControllerClass;
                    if (menuOperation != null)
                    {

                        menuOperation.ShowScreen(EMenuType.MainMenu, true);
                        return;

                    }
                }
                Logger.LogError("Could not retrive Area Data");
            }
        }
    }
}