using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;

namespace SmugglersPack.Patches
{
    public class Logic_ThiefPatches
    {
        // Disabled because this is used for logic like a shopkeeper saying "Hey, you need to pay for that before you leave"
        // Doesn't make sense for the backpack to hide that you picked up an item they're selling
        /*[HarmonyPatch(typeof(Logic_Thief), nameof(Logic_Thief.CheckForStolenGoods))]
        [HarmonyDebug]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new CodeMatcher(instructions);
            CodeInstruction instructionInventoryAllSlots = CodeInstruction.Call("Inventory:AllSlots");
            instructionInventoryAllSlots.opcode = OpCodes.Callvirt;

            CodeMatch Inventory_AllSlots = new CodeMatch(instructionInventoryAllSlots);

            Plugin.Log.LogInfo(Inventory_AllSlots);

            codeMatcher.MatchForward(true, [Inventory_AllSlots])
                .ThrowIfInvalid("Unable to find Inventory.AllSlots search in Logic_Thief")
                .InsertAndAdvance(CodeInstruction.Call("SmugglersPack.SmugglerPackController:PruneHiddenBackpackSlots"));
            
            Plugin.Log.LogInfo(" Patched Logic_Thief");

            return codeMatcher.Instructions();
        }*/
    }
}
