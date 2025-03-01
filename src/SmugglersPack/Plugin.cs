using BepInEx;
using BepInEx.Logging;
using SmugglersPack.Settings;
using HarmonyLib;
using Lavender;
using SmugglersPack.Patches;
using System.Reflection;
using System.IO;

namespace SmugglersPack;

[BepInPlugin(LCMPluginInfo.PLUGIN_GUID, LCMPluginInfo.PLUGIN_NAME, LCMPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log = null!;
    internal static SmugglersPackSettings Settings = null!;

    private void Awake()
    {
        Log = Logger;
        Settings = new(Config);

        // Log our awake here so we can see it in LogOutput.txt file
        Log.LogInfo($"Plugin {LCMPluginInfo.PLUGIN_NAME} version {LCMPluginInfo.PLUGIN_VERSION} is loaded!");

        string itemsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "items.json");
        Lavender.Lavender.AddCustomItemsFromJson(itemsPath, LCMPluginInfo.PLUGIN_NAME);
        string recipesPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "recipes.json");
        Lavender.Lavender.AddCustomRecipesFromJson(recipesPath, LCMPluginInfo.PLUGIN_NAME);

        Harmony myHarmony = new(LCMPluginInfo.PLUGIN_GUID);
        myHarmony.PatchAll(typeof(InventoryPatches));
        myHarmony.PatchAll(typeof(StoragePatches));
        myHarmony.PatchAll(typeof(ItemUIPatches));

        ItemUIPatches.Load();
    }

}
