using EFT.InventoryLogic;
using EFT.UI;
using EFT.UI.DragAndDrop;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static EFT.UI.SimpleStashPanel;
using static UnityEngine.UI.Image;

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
            var backpackSlotItemView = backpackSlot.GetComponentInChildren<SlotItemView>(true);

            if (backpackSlotItemView != null)
            {
                var canvasGroup = backpackSlotItemView.GetComponent<CanvasGroup>();
                var headerImage = backpackSlot.GetComponentInChildren<SlotViewHeader>(true).transform.GetChild(0).GetChild(1).GetComponent<Image>();

                if (canvasGroup != null && headerImage != null)
                {
                    canvasGroup.blocksRaycasts = false;
                    headerImage.raycastTarget = false;
                }
            }
        }

        private static void HideBackpackGrids(GameObject backpackSlot)
        {
            var backpackGridView = backpackSlot.GetComponentInChildren<GridView>(true);

            if (backpackGridView != null)
            {
                backpackGridView.gameObject.SetActive(false);
            }
        }

        private static void HideBackground(GameObject backpackSlot)
        {
            var backpackSlotItemView = backpackSlot.GetComponentInChildren<SlotItemView>(true);

            if (backpackSlotItemView != null)
            {
                var backgroundImage = backpackSlotItemView.gameObject.transform.Find("Background").gameObject;

                if (backgroundImage != null)
                {
                    backgroundImage.SetActive(false);
                }
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

                        HideBackpackGrids(child.gameObject);
                        HideBackpackInteraction(child.gameObject);
                        HideBackground(child.gameObject);

                        Destroy(this);
                    }
                }
            }
        }
    }
}