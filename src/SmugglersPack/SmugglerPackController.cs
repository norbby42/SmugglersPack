using System;
using System.Collections.Generic;
using System.Text;
using Lavender;
using static StorageModelController;

namespace SmugglersPack
{
    public class SmugglerPackController
    {
        static SmugglerPackController Instance = new SmugglerPackController();

        public static ItemReference SmugglerPackInsert = new ItemReference(400001);

        public static string SmugglerPackInsertCategory = "Backpack Insert Last Slot";

        public static bool IsSmugglersInsertEquipped(Storage? storage = null)
        {
            if (storage == null)
            {
                storage = BackpackStorage.instance.GetStorage();
            }
            if (storage != null)
            {
                int lastIndex = storage.customStorageSlotAmount - 1;
                if (storage.Slots.Length >= lastIndex)
                {
                    SlotController slotController = storage.Slots[lastIndex];
                    return slotController != null && slotController.itemStack != null && slotController.itemStack.itemId == SmugglerPackInsert.ID;
                }
            }

            return false;
        }

        public static bool IsProtectedSlot(SlotController slotController)
        {
            if (Storage.active != null)
            {
                var index = Array.IndexOf(Storage.active.Slots, slotController);
                if (index != -1 &&
                    SmugglerPackController.IsBackpackStorage(Storage.active) &&
                    SmugglerPackController.IsSmugglersInsertEquipped(Storage.active))
                {
                    return index >= SmugglerPackController.GetFirstHiddenSlotIndex(Storage.active);
                }
            }
            return false;
        }

        public static int GetFirstHiddenSlotIndex(Storage storage)
        {
            int slotCount = storage.Slots.Length;

            return Math.Max(Math.Min(slotCount - Plugin.Settings.NumberHiddenSlots.Value, slotCount), 0);
        }

        public static SlotController[] PruneHiddenBackpackSlots(SlotController[] slots)
        {
            if (IsSmugglersInsertEquipped())
            {
                List<SlotController> slotsList = new List<SlotController>(slots);
                bool changed = false;
                for (int i = GetFirstHiddenSlotIndex(BackpackStorage.instance.GetStorage()); i < BackpackStorage.instance.GetStorage().Slots.Length; ++i)
                {
                    SlotController slot = BackpackStorage.instance.GetStorage().Slots[i];
                    if (slot.itemStack != null && slot.itemStack.itemId != 0)
                    {
                        slotsList.Remove(slot);
                        changed = true;
                        Plugin.Log.LogInfo($"Removed item {slot.itemStack.itemId} from allitems list because it is hidden in backpack.");
                    }
                }

                if (changed)
                {
                    return slotsList.ToArray();
                }
            }

            return slots;
        }

        public static List<ItemStack> PruneHiddenBackpackSlots(List<ItemStack> slots)
        {
            if (SmugglerPackController.IsSmugglersInsertEquipped())
            {
                for (int i = SmugglerPackController.GetFirstHiddenSlotIndex(BackpackStorage.instance.GetStorage()); i < BackpackStorage.instance.GetStorage().Slots.Length; ++i)
                {
                    SlotController slot = BackpackStorage.instance.GetStorage().Slots[i];
                    if (slot.itemStack != null && slot.itemStack.itemId != 0)
                    {
                        slots.Remove(slot.itemStack);
                        Plugin.Log.LogInfo($"Removed item {slot.itemStack.itemId} from stolen items list because it is hidden in the backpack.");
                    }
                }
            }

            return slots;
        }

        public static bool IsBackpackStorage(Storage storage)
        {
            // An equipped backpack storage?
            if (BackpackStorage.instance.GetStorage() == storage)
            {
                return true;
            }
            // Backpack on the ground?
            else if (storage.GetComponentInParent<BackpackCollectibleItem>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
