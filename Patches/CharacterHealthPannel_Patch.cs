using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UIRefresh.Config;
using UnityEngine;

namespace UIRefresh.Patches
{
    internal class CharacterHealthPanel_Patch : ModulePatch
    {
        public static CharacterHealthPanel? characterHealthPanel = null;
        private static Vector2 defaultPosition;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(CharacterHealthPanel), "Show");
        }

        [PatchPostfix]
        static void Postfix(CharacterHealthPanel __instance)
        {
            characterHealthPanel = __instance;
            if (characterHealthPanel != null)
            {
                if (defaultPosition != null)
                {
                    MoveCharacterHealthPanel();
                    return;
                }
                defaultPosition = characterHealthPanel.gameObject.RectTransform().anchoredPosition;
                MoveCharacterHealthPanel();
            }
        }

        public static void MoveCharacterHealthPanel()
        {
            if (characterHealthPanel != null)
            {
                if (Plugin.Instance.UIRefreshConfig.MoveHealthPanelConfig.Value)
                {
                    characterHealthPanel.gameObject.RectTransform().anchoredPosition = new Vector2(61, -820);
                    characterHealthPanel.gameObject.RectTransform().localScale = new Vector3(0.75f, 0.75f, 0.75f);
                }
                else
                {
                    characterHealthPanel.gameObject.RectTransform().anchoredPosition = defaultPosition;
                    characterHealthPanel.gameObject.RectTransform().localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }
}