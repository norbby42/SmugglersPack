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
            return SmugglerPackController.PruneHiddenBackpackSlots(__result);
        }
    }
}
