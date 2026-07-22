using EFT.UI;
using SPT.Reflection.Patching;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIRefresh.Patches
{
    internal class AmmoPannel_ShowPatch : ModulePatch
    {
        public static GameObject? AmmoCountPanelObject = null;
        public static TextMeshProUGUI[]? AmmoPanelTextMeshProUGUI = null;


        protected override MethodBase GetTargetMethod()
        {
            return typeof(AmmoCountPanel).GetMethod(nameof(AmmoCountPanel.Show));
        }

        [PatchPostfix]
        static void Postfix(AmmoCountPanel __instance)
        {
            AmmoPanelTextMeshProUGUI = __instance.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
            var AmmoPanelBG = __instance.gameObject.GetComponent<Image>();
            AmmoPanelBG.ChangeImageAlpha(0);

            AmmoPanelUpdate();
        }

        public static void AmmoPanelUpdate()
        {
            if (AmmoPanelTextMeshProUGUI != null)
            {
                foreach(TextMeshProUGUI ammoText in AmmoPanelTextMeshProUGUI)
                {
                    ammoText.alpha = Plugin.Instance.UIRefreshConfig.AmmoPanelAlpha.Value;
                }
            }
        }
    }
}