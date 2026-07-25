using EFT;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace UIRefresh.Patches
{
    internal class BattleStancePanel_ShowPatch : ModulePatch
    {
        public static GameObject? BattleStancePanel = null;

        public static GameObject? RaidWarningTimerObject = null;


        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(BattleStancePanel), "Show");
        }

        [PatchPostfix]
        static void Postfix(BattleStancePanel __instance,Player player)
        {
            BattleStancePanel = __instance.gameObject;

            if (BattleStancePanel != null)
            {
                StanceSillhouetteUpdate();
                UpdateNoiseLevel();
                UpdateStanceSlider();
                UpdateSpeedSlider();

            }
        }

        public static void StanceSillhouetteUpdate()
        {
            if (BattleStancePanel != null)
            {
                var stances = BattleStancePanel.transform.Find("Stances").gameObject;

                stances.SetActive(!Plugin.Instance.UIRefreshConfig.HideStanceSillhouette.Value);
            }
        }

        public static void UpdateNoiseLevel()
        {
            if (BattleStancePanel != null)
            {
                var noiseLevel = BattleStancePanel.transform.Find("SprintBar/NoiseLevel").gameObject;
                noiseLevel.SetActive(!Plugin.Instance.UIRefreshConfig.DisableNoiseLevel.Value);

            }
        }
        public static void UpdateStanceSlider()
        {
            if (BattleStancePanel != null)
            {
                var StanceSlider = BattleStancePanel.transform.Find("StanceSlider").gameObject;
                StanceSlider.SetActive(!Plugin.Instance.UIRefreshConfig.DisableStanceSlider.Value);

            }
        }

        public static void UpdateSpeedSlider()
        {
            if (BattleStancePanel != null)
            {
                var SpeedSlider = BattleStancePanel.transform.Find("SpeedSlider").gameObject;
                SpeedSlider.SetActive(!Plugin.Instance.UIRefreshConfig.DisableSpeedSlider.Value);

            }
        }
    }
}