using EFT;
using EFT.UI;
using EFT.UI.Gestures;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace UIRefresh.Patches
{
    internal class AmmoPannel_ShowPatch : ModulePatch
    {
        public static GameObject? AmmoCountPanelObject = null;
        public static GameObject? AmmoPanel = null;


        protected override MethodBase GetTargetMethod()
        {
            return typeof(AmmoCountPanel).GetMethod(nameof(AmmoCountPanel.Show));
        }

        [PatchPostfix]
        static void Postfix(AmmoCountPanel __instance)
        {

            AmmoCountPanelObject = __instance.gameObject;
            AmmoPanel = AmmoCountPanelObject.transform.Find("Ammo").gameObject;

            if (AmmoPanel != null)
            {
                AmmoPanelUpdate();

            }
        }

        public static void AmmoPanelUpdate()
        {
            if (AmmoPanel != null)
            {
                AmmoPanel.SetActive(!Plugin.Instance.UIRefreshConfig.HideAmmoPanel.Value);
            }
        }
    }
}