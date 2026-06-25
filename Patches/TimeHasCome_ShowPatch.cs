
using EFT;
using EFT.UI;
using EFT.UI.Matchmaker;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIRefresh.Patches
{
    // 6. Raid Loading Screen
    internal class TimeHasCome_ShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchmakerTimeHasCome), "Show", new System.Type[] { typeof(ISession), typeof(RaidSettings), typeof(MatchmakerPlayerControllerClass) });
        }

        [PatchPostfix]
        static void Postfix(MatchmakerBannersPanel ____bannersPanel, MatchmakerTimeHasCome __instance)
        {
            Plugin.initOnce = false;
            PreloaderUI preloaderUI = MonoBehaviourSingleton<PreloaderUI>.Instance;

            var locationNamePanel = __instance.transform.Find("Location Name Panel");
            var locationNameGUI = locationNamePanel.gameObject.transform.Find("Name").GetComponent<CustomTextMeshProUGUI>();
            var accentColor = Utils.GetMapColorConfig(locationNameGUI.text);

            if (Plugin.MenuLayoutChangesConfig.Value)
            {
                preloaderUI.SetMenuTaskBarVisibility(false);


                //Deactivate banner, logo, tips, and loading spinner.
                var bannerPanel = ____bannersPanel.gameObject;
                if (bannerPanel != null)
                {
                    bannerPanel.gameObject.SetActive(false);
                }

                // Hide Header Text
                var captionsHolder = __instance.transform.Find("CaptionsHolder");
                if (captionsHolder != null)
                {
                    captionsHolder.gameObject.SetActive(false);
                }

                //Hide EFT Logo
                var logo = __instance.transform.Find("Logo");
                if (logo != null)
                {
                    logo.gameObject.SetActive(false);
                }

                //Hide Loading Spinner
                var loader = __instance.transform.Find("Loader");
                if (loader != null)
                {
                    loader.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(925, 30);
                    //loader.gameObject.SetActive(false);
                }

                //Other Players in Raid list - move
                var partyInfoPanel = __instance.transform.Find("PartyInfoPanel");
                if (partyInfoPanel != null)
                {
                    partyInfoPanel.GetComponent<RectTransform>().localPosition = new Vector2(-922, 550);
                }

                //Map Name - move, increase font, set background to match.
                if (locationNamePanel != null)
                {
                    locationNamePanel.gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-850, 950);


                    locationNameGUI.alignment = TextAlignmentOptions.Center;
                    locationNameGUI.fontSize = 130;

                    var locationNameText = locationNameGUI.text;

                    locationNameGUI.color = accentColor.Value;

                    locationNamePanel.gameObject.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(450, 150);
                    locationNamePanel.gameObject.transform.Find("Background").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

                    Utils.GetMapColorConfig(locationNameGUI.text).SettingChanged += (s, e) =>
                    {
                        locationNameGUI.color = Utils.GetMapColorConfig(locationNameGUI.text).Value;
                    };

                    if (Plugin.ChangeUISceneOnLoading.Value)
                    {
                        Utils.setLoadRaidBackground(locationNameGUI.text);
                    }
                }

                //Loading Status Text transform and alignment:
                var deployCaption = __instance.transform.Find("Deploying Caption");
                if (deployCaption != null)
                {
                    deployCaption.gameObject.GetComponent<RectTransform>().position = new Vector3(340, 26, 0);
                    deployCaption.gameObject.GetComponent<CustomTextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
                }

                //Start & Back Button transform:
                var startBackButton = __instance.transform.Find("Back Button Panel");
                if (startBackButton != null)
                {
                    startBackButton.gameObject.GetComponent<RectTransform>().position = new Vector3(1280, 20, 0);
                }

                //Player Model transform and lights:
                var playerModel = __instance.transform.Find("PlayerModelView");
                if (playerModel != null)
                {

                    playerModel.gameObject.GetComponent<RectTransform>().position = new Vector3(2150f, 0f, 0f);
                    //playerModel.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 0f);
                    //playerModel.gameObject.GetComponent<AspectRatioFitter>().aspectRatio = 3.3f;

                    var playerModelCamera = playerModel.Find("PlayerModelViewObject").Find("Camera_timehascome0");
                    if (playerModelCamera != null)
                    {
                        playerModelCamera.GetComponent<Camera>().fieldOfView = 28;
                    }

                    //Player Model Lights:
                    var playerModellights = playerModel.Find("PlayerModelViewObject").Find("Lights");
                    if (playerModellights != null)
                    {
                        playerModellights.GetChild(2).gameObject.SetActive(false);
                        playerModellights.GetChild(3).gameObject.SetActive(false);

                        var mainLight = playerModellights.Find("Main Light");
                        mainLight.GetComponent<Transform>().localEulerAngles = new Vector3(60f, 60f, 20f);
                        mainLight.GetComponent<Transform>().localPosition = new Vector3(-0.243f, -294.1938f, 4.65f);
                        mainLight.GetComponent<Light>().type = LightType.Spot;
                        mainLight.GetComponent<Light>().spotAngle = 65;
                        mainLight.GetComponent<Light>().range = 1.3f;
                        mainLight.GetComponent<Light>().intensity = 6f;
                        mainLight.GetComponent<Light>().color = new Color(1, 0.86f, 0.74f, 1);

                        var fillLight = playerModellights.Find("Fill Light");
                        fillLight.GetComponent<Transform>().localEulerAngles = new Vector3(0f, 130f, 200f);
                        //fillLight.GetComponent<Light>().intensity = 0.3;
                        fillLight.GetComponent<Light>().color = accentColor.Value;

                        Utils.GetMapColorConfig(locationNameGUI.text).SettingChanged += (s, e) =>
                        {
                            fillLight.GetComponent<Light>().color = Utils.GetMapColorConfig(locationNameGUI.text).Value;
                        };
                    }
                }

                if (Plugin.HideOutMainMenuConfig.Value)
                {
                    Utils.ShowEnvironmentUI(true);
                    Utils.ToggleEnvironmentBackground(true);
                }
            }
        }
    }
}