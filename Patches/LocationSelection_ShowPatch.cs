
using EFT;
using EFT.UI.Matchmaker;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UIRefresh.Patches
{
    internal class LocationSelection_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchMakerSelectionLocationScreen), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(MatchmakerPlayerControllerClass) });
        }

        [PatchPostfix]
        static void Postfix(MatchMakerSelectionLocationScreen __instance)
        {
            if (Plugin.Instance.UIRefreshConfig.MenuLayoutChangesConfig.Value)
            {
                __instance.transform.Find("CaptionsHolder").gameObject.SetActive(false);

                var map = __instance.transform.Find("Content").GetChild(1);
                map.GetComponent<RectTransform>().anchoredPosition = new Vector2(950, 100);
                map.GetComponent<RectTransform>().sizeDelta = new Vector2(796, 0);

                var conditionsPannel = __instance.transform.Find("Conditions Panel");
                conditionsPannel.Find("Tiles").gameObject.SetActive(false);
                conditionsPannel.GetComponent<Image>().enabled = false;
                conditionsPannel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-440, 470);

                var locationInfoPanel = __instance.transform.Find("Content").Find("Location Info Panel");
                locationInfoPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-800, -100);
                locationInfoPanel.Find("Banner").GetComponent<RectTransform>().localScale = new Vector3(1.9f, 1.9f, 1);
                locationInfoPanel.Find("Banner").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 120);

                var descriptionPanel = locationInfoPanel.transform.Find("DescriptionPanel");
                descriptionPanel.Find("Bottom Panel").Find("IconsPanel").Find("Difficulty").gameObject.SetActive(false);
                descriptionPanel.Find("Bottom Panel").Find("IconsPanel").Find("Players").gameObject.SetActive(false);
                descriptionPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-45, 40);
                
                var locationDescription = descriptionPanel.Find("Location Description");
                locationDescription.GetComponent<CustomTextMeshProUGUI>().fontSize = 17;
                locationDescription.GetComponent<CustomTextMeshProUGUI>().color = new Color(0.7647f, 0.7725f, 0.698f, 1);
            }
        }
    }
}