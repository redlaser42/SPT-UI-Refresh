
using EFT.HealthSystem;
using EFT.InventoryLogic;
using EFT.UI;
using EFT.UI.Matchmaker;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace UIRefresh.Patches
{
    // 4. Insurance Menu
    internal class InsuranceScreen_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchmakerInsuranceScreen), "Show", new System.Type[] { typeof(AbstractQuestControllerClass), typeof(IHealthController), typeof(InventoryController), typeof(ISession) });
        }

        [PatchPostfix]
        static void Postfix(MatchmakerInsuranceScreen __instance)
        {
            if (Plugin.MenuLayoutChangesConfig.Value)
            {
                __instance.transform.Find("WarningPanel").gameObject.SetActive(false);
                __instance.transform.Find("Tab Bar").GetComponent<RectTransform>().anchoredPosition = new Vector2(1, -100);
                __instance.transform.Find("ItemsToInsurePanel").GetComponent<RectTransform>().sizeDelta = new Vector2(5, -305);
                __instance.transform.Find("ItemsToInsurePanel").GetComponent<RectTransform>().anchoredPosition = new Vector2(-3.5f, 20);
                __instance.transform.Find("ItemsToInsurePanel").Find("ItemsToInsureList").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -80);
                __instance.transform.Find("ItemsToInsurePanel").Find("ItemsToInsureList").GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 490);
            }

            if (Plugin.SkipPreRaidMenusConfig.Value)
            {
                var nextButton = __instance.transform.Find("ScreenDefaultButtons/NextButton/");
                nextButton.GetComponent<DefaultUIButton>().method_11();
            }

            if (Plugin.HideOutMainMenuConfig.Value)
            {
                GameObject fpsCAM = Plugin.FindFPSCam();
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