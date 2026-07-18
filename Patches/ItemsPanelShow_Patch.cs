
using EFT.UI;
using EFT.UI.DragAndDrop;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UnityEngine;
using static EFT.UI.SimpleStashPanel;
using EFT.InventoryLogic;

namespace UIRefresh.Patches
{
    internal class ItemsPanelShow_Patch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(ItemsPanel).GetMethod(nameof(ItemsPanel.Show));
        }

        [PatchPostfix]
        static void Postfix(ItemsPanel __instance, bool inRaid, SimpleStashPanel.EStashSearchAvailability searchAvailability,CompoundItem lootItem, ContainersPanel ____containers)
        {
            var isPlayerLooting = searchAvailability != EStashSearchAvailability.Unavailable || lootItem != null; ;

            if (Plugin.Instance.UIRefreshConfig.HideBackpackInventory.Value && inRaid && !isPlayerLooting)
            {
                var content = ____containers.transform.Find("Content");
                var backpackWatcher = content.GetComponent<BackpackWatcher>();

                if (backpackWatcher == null)
                {
                    backpackWatcher = content.gameObject.AddComponent<BackpackWatcher>();
                }
            }
        }

        private static void HideBackpackInteraction(GameObject backpackSlot)
        {
            var backpackSlotItemView = backpackSlot.transform.GetChild(1).GetChild(4).gameObject.GetComponent<SlotItemView>();

            if (backpackSlotItemView != null)
            {
                var canvasGroup = backpackSlotItemView.GetComponent<CanvasGroup>();

                if (canvasGroup != null)
                {
                    canvasGroup.blocksRaycasts = false;
                }
            }
        }

        private static void HideBackpackGrids(GameObject backpackSlot)
        {
            if (backpackSlot.transform.GetChild(5).GetChild(0).gameObject != null)
            {
                backpackSlot.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
            }
        }

        public class BackpackWatcher : MonoBehaviour
        {
            private bool _found;
            private void OnTransformChildrenChanged()
            {
                if (_found)
                    return;
                foreach (Transform child in GetComponentsInChildren<Transform>(true))
                {
                    if (child.name == "Backpack Slot")
                    {
                        _found = true;
                        HideBackpackInteraction(child.gameObject);
                        HideBackpackGrids(child.gameObject);

                        Destroy(this);
                    }
                }
            }
        }
    }
}