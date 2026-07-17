using EFT;
using EFT.UI;
using EFT.UI.Matchmaker;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace UIRefresh.Patches
{
    // 3. Offline Raid Settings Menu
    internal class RaidSettingsScreen_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchmakerOfflineRaidScreen), "Show", new System.Type[] { typeof(InfoClass), typeof(RaidSettings), typeof(RaidSettings) });
        }

        [PatchPostfix]
        static void Postfix(MatchmakerOfflineRaidScreen __instance)
        {
            if (Plugin.Instance.UIRefreshConfig.MenuLayoutChangesConfig.Value)
            {
                // Deactivates top text and spacers.
                __instance.transform.Find("Content").Find("Description").gameObject.SetActive(false);
                __instance.transform.Find("Content").Find("Space (1)").gameObject.SetActive(false);
                __instance.transform.Find("Content").Find("Space (2)").gameObject.SetActive(false);
            }
            else
            {
                __instance.transform.Find("Content").Find("Description").gameObject.SetActive(true);
                __instance.transform.Find("Content").Find("Space (1)").gameObject.SetActive(true);
                __instance.transform.Find("Content").Find("Space (2)").gameObject.SetActive(true);
            }

            if (Plugin.Instance.UIRefreshConfig.SkipPreRaidMenusConfig.Value)
            {
                var nextButton = __instance.transform.Find("ScreenDefaultButtons/NextButton/");
                nextButton.GetComponent<DefaultUIButton>().method_11();
            }
        }
    }
}