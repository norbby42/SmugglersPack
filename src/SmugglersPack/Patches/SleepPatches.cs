using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmugglersPack.Patches
{
    public class SleepPatches
    {
        [HarmonyPatch(typeof(SleepEventController), nameof(SleepEventController.AllowedToRemove))]
        [HarmonyPostfix]
        public static bool SleepEventController_AllowedToRemove_Postfix(bool __result, ItemStack itemStack)
        {
            if (!__result)
            {
                return __result;
            }

            // Prune using the same criteria as hiding items from crime
            List<ItemStack> pruned = SmugglerPackController.PruneHiddenBackpackSlots([itemStack]);

            // If there's still an item after pruning, then it's removable (ie not hidden by the insert)
            return pruned.Count > 0;
        }
    }
}
