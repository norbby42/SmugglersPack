using System;
using System.Collections.Generic;
using System.Text;
using Lavender;

namespace SmugglersPack
{
    public class SmugglerPackController
    {
        static SmugglerPackController Instance = new SmugglerPackController();

        public static ItemReference SmugglerPackEmpty = new ItemReference(100001);
        public static ItemReference SmugglerPack = new ItemReference(100002);

        public static bool IsSmugglersPackEquipped()
        {
            foreach (SlotController slot in Inventory.instance.CharacterSlots)
            {
                if (slot.itemStack != null && (slot.itemStack.itemId == SmugglerPack.ID || slot.itemStack.itemId == SmugglerPackEmpty.ID))
                {
                    return true;
                }
            }

            return false;
        }

        public static int GetFirstHiddenSlotIndex()
        {
            if (!IsSmugglersPackEquipped())
            {
                return 9999;
            }

            int slotCount = BackpackStorage.instance.backpackStorage.Slots.Length;

            return Math.Max(Math.Min(slotCount - Plugin.Settings.NumberHiddenSlots.Value, slotCount), 0);
        }

        public static SlotController[] PruneHiddenBackpackSlots(SlotController[] slots)
        {
            if (IsSmugglersPackEquipped())
            {
                List<SlotController> slotsList = new List<SlotController>(slots);
                bool changed = false;
                for (int i = GetFirstHiddenSlotIndex(); i < BackpackStorage.instance.backpackStorage.Slots.Length; ++i)
                {
                    SlotController slot = BackpackStorage.instance.backpackStorage.Slots[i];
                    if (slot.itemStack != null && slot.itemStack.itemId > -1)
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
    }
}
