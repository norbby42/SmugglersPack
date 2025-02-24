using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace SmugglersPack.Patches
{
    public class InventoryPatches
    {
        [HarmonyPatch(typeof(Inventory), nameof(Inventory.GetStolenItems))]
        [HarmonyPostfix]
        static List<ItemStack> Inventory_GetStolenItems_Postfix(List<ItemStack> __result, int ownerId, Inventory __instance)
        {
            if (SmugglerPackController.IsSmugglersPackEquipped())
            {
                for (int i = SmugglerPackController.GetFirstHiddenSlotIndex(); i < BackpackStorage.instance.backpackStorage.Slots.Length; ++i)
                {
                    SlotController slot = BackpackStorage.instance.backpackStorage.Slots[i];
                    if (slot.itemStack != null && slot.itemStack.itemId > -1)
                    {
                        __result.Remove(slot.itemStack);
                        Plugin.Log.LogInfo($"Removed item {slot.itemStack.itemId} from stolen items list because it's hidden in the backpack.");
                    }
                }
            }

            return __result;
        }
    }
}
