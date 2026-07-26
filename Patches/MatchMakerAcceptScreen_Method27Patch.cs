using EFT;
using EFT.UI;
using EFT.UI.Matchmaker;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;


namespace UIRefresh.Patches
{
    //Hides the PVP to PVE toggle button in the bottom right. 
    internal class MatchMakerAcceptScreenMethod_27_Patch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchMakerAcceptScreen), nameof(MatchMakerAcceptScreen.method_27));
        }

        [PatchPrefix]
        static bool Prefix(MatchMakerAcceptScreen __instance)
        {
            if (Plugin.Instance.UIRefreshConfig.SkipPreRaidMenusConfig.Value)
            {
                __instance.gameObject.SetActive(false);

                TarkovApplication.Exist(out TarkovApplication tarkovApp);
                if (tarkovApp != null)
                {
                    var menuOperation = AccessTools.Field(tarkovApp.GetType(), "mainMenuControllerClass").GetValue(tarkovApp) as MainMenuControllerClass;
                    if (menuOperation != null)
                    {
                        menuOperation.ShowScreen(EMenuType.MainMenu, true);
                    }
                }
                return false;
            }
            else 
            { 
                return true;
            }
        }

    }
}