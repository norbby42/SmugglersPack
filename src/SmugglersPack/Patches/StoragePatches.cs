using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace SmugglersPack.Patches
{
    public class StoragePatches
    {
        [HarmonyPatch(typeof(Storage), nameof(Storage.Start))]
        [HarmonyPostfix]
        public static void Storage_Start_Postfix(Storage __instance)
        {
            if (SmugglerPackController.IsBackpackStorage(__instance))
            {
                if (!__instance.ForbiddenCategories.Contains(SmugglerPackController.SmugglerPackInsertCategory))
                {
                    __instance.ForbiddenCategories = __instance.ForbiddenCategories.AddToArray(SmugglerPackController.SmugglerPackInsertCategory);
                }
                // When a backpack is placed onto the ground, or immediately after it is picked up, it runs Start but *DOES NOT* run GetStorage
                // So we have to manually run that logic here to be certain that we will ensure correct placement rules when dropping/picking up a backpack with an insert
                Storage_GetStorage_Postfix(__instance);
            }
        }

        [HarmonyPatch(typeof(Storage), nameof(Storage.GetStorage))]
        [HarmonyPostfix]
        public static void Storage_GetStorage_Postfix(Storage __instance)
        {
            // When opening the backpack, we need to explicitly remove the smuggler pack insert category from the final slot's forbidden categories list
            // This is how we prevent putting the insert into any other slot in the backpack
            if (SmugglerPackController.IsBackpackStorage(__instance))
            {
                for (int i = 0; i < __instance.Slots.Length - 1; i++)
                {
                    var slot = __instance.Slots[i];
                    if (slot != null)
                    {
                        if (!slot.ForbiddenCategories.Contains(SmugglerPackController.SmugglerPackInsertCategory))
                        {
                            slot.SetForbiddenCategories(slot.ForbiddenCategories.AddToArray(SmugglerPackController.SmugglerPackInsertCategory).ToArray());
                        }
                    }
                }

                SlotController finalSlot = __instance.Slots[__instance.Slots.Length - 1];
                if (finalSlot != null)
                {
                    finalSlot.SetForbiddenCategories(finalSlot.ForbiddenCategories.Except([SmugglerPackController.SmugglerPackInsertCategory]).ToArray());
                }
            }
        }

        [HarmonyPatch(typeof(CollectibleItem), nameof(CollectibleItem.UpdateBackpack))]
        [HarmonyPrefix]
        public static void CollectibleItem_UpdateBackpack_Prefix(CollectibleItem __instance)
        {
            Storage storage = BackpackStorage.instance.GetStorage(__instance.Item.Item.ID);
            if (storage != null && __instance.GetComponentInChildren<BackpackCollectibleItem>())
            {
                // Init slot permissions before we move items into the backpack
                Storage_GetStorage_Postfix(storage);
            }
        }
    }
}
