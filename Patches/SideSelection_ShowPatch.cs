using EFT;
using EFT.HealthSystem;
using EFT.InventoryLogic;
using EFT.UI.Matchmaker;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UIRefresh.Config;
using UnityEngine;

namespace UIRefresh.Patches
{
    // 1. SCAV or PMC Selection Menu
    internal class SideSelection_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchMakerSideSelectionScreen), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(IHealthController), typeof(InventoryController) });
        }

        [PatchPostfix]
        static void Postfix(MatchMakerSideSelectionScreen __instance)
        {
            if (Plugin.Instance.UIRefreshConfig.MenuLayoutChangesConfig.Value)
            {
                // Deactivates Logo and texts.
                __instance.transform.Find("CaptionsHolder").gameObject.SetActive(false);
                __instance.transform.Find("Logo").gameObject.SetActive(false);
                __instance.transform.Find("Description").gameObject.SetActive(false);

                // Adjust PMC model
                var PMC = __instance.transform.Find("PMCs");
                PMC.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, -100);
                PMC.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 300);

                //PMC.transform.Find("PMCPlayerMV").GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 500);
                //PMC.transform.Find("PMCPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Camera>().fieldOfView = 20;
                //PMC.transform.Find("PMCPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Transform>().localPosition = new Vector3(0.62f, 0.4f, 0.96f);
                //PMC.transform.Find("PMCPlayerMV").GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 0);
                //PMC.transform.Find("PMCPlayerMV").Find("PlayerMVObject").Find("MenuPlayer").GetComponent<Transform>().localPosition = new Vector3(0.62f, 0.4f, 0.96f);


                //Adjust SCAV model
                var SCAV = __instance.transform.Find("Savage");
                SCAV.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -100);
                SCAV.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 300);

                //SCAV.transform.Find("ScavPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Camera>().fieldOfView = 20;
                //SCAV.transform.Find("ScavPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Transform>().localPosition = new Vector3(-1.71f, 0.16f, 1.28f);
                //SCAV.transform.Find("ScavPlayerMV").GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 0);
                //SCAV.transform.Find("ScavPlayerMV").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 320);
                //SCAV.transform.Find("ScavPlayerMV").Find("PlayerMVObject").Find("Camera_matchmaker").GetComponent<Transform>().localEulerAngles = new Vector3(0, 200, 0);
            }
            else
            {
                __instance.transform.Find("CaptionsHolder").gameObject.SetActive(true);
                __instance.transform.Find("Description").gameObject.SetActive(true);
            }
        }
    }
}