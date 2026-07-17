
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
            var isPlayerLooting = false;

            if (searchAvailability != EStashSearchAvailability.Unavailable || lootItem != null)
            {
                isPlayerLooting = true;
            }

            if (Plugin.Instance.UIRefreshConfig.HideBackpackInventory.Value && inRaid && !isPlayerLooting)
            {
                var content = ____containers.transform.Find("Content");

                //backpack slot hasn't quite yet been created yet, so we add a watcher that will find the backpack slot when it get's added. 
                var backpackWatcher = content.gameObject.AddComponent<BackpackWatcher>();

                backpackWatcher.OnBackpackCreated += backpackGO =>
                {
                    if (backpackGO != null)
                    {
                        HideBackpackInteraction(backpackGO);
                        HideBackpackGrids(backpackGO);
                    }
                };
            }
        }

        private static void HideBackpackInteraction(GameObject backpackSlot)
        {
            var backpackItemView = backpackSlot.transform.GetChild(1).GetChild(4).gameObject.GetComponent<SlotItemView>();

            if (backpackItemView != null)
            {
                backpackItemView.enabled = false;
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
            public Action<GameObject> OnBackpackCreated;
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
                        OnBackpackCreated?.Invoke(child.gameObject);

                        Destroy(this);
                    }
                }
            }
        }
    }
}