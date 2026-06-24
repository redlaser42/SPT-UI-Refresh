
using EFT;
using EFT.UI.Matchmaker;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UIRefresh.Patches
{
    // 2. Level Select Map Menu
    internal class LocationSelection_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchMakerSelectionLocationScreen), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(MatchmakerPlayerControllerClass) });
        }

        [PatchPostfix]
        static void Postfix(MatchMakerSelectionLocationScreen __instance)
        {
            if (Plugin.MenuLayoutChangesConfig.Value)
            {
                __instance.transform.Find("CaptionsHolder").gameObject.SetActive(false);

                var conditionsPannel = __instance.transform.Find("Conditions Panel");
                conditionsPannel.Find("Tiles").gameObject.SetActive(false);
                conditionsPannel.GetComponent<Image>().enabled = false;
                conditionsPannel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-440, 470);

                var locationInfo = __instance.transform.Find("Content").GetChild(0);
                locationInfo.GetComponent<RectTransform>().anchoredPosition = new Vector2(-800, -100);
                locationInfo.Find("Banner").GetComponent<RectTransform>().localScale = new Vector3(1.76f, 1.76f, 1);
                locationInfo.Find("DescriptionPanel").GetChild(0).GetComponent<CustomTextMeshProUGUI>().fontSize = 16;
                locationInfo.Find("DescriptionPanel").GetChild(1).GetChild(2).gameObject.SetActive(false);
                locationInfo.Find("DescriptionPanel").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);

                var map = __instance.transform.Find("Content").GetChild(1);
                map.GetComponent<RectTransform>().anchoredPosition = new Vector2(950, 100);
                map.GetComponent<RectTransform>().sizeDelta = new Vector2(796, 0);
            }
            else
            {
                __instance.transform.Find("CaptionsHolder").gameObject.SetActive(true);
            }
        }
    }
}