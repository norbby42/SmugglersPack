using BepInEx.Configuration;
using UnityEngine;

namespace SmugglersPack.Settings;

public class SmugglersPackSettings(ConfigFile config)
{
    public ConfigEntry<int> NumberHiddenSlots = config.Bind<int>("SmugglersPack", "NumberHiddenSlots", 8, "The number of slots hidden by the smuggler's pack insert.  Valid range: (1, 24)");

    public ConfigEntry<Color> HiddenSlotColor = config.Bind<Color>("SmugglersPack", "HiddenSlotIconColor", new Color(1f, 1f, 1f, 0.4f), "Color and alpha tint for the mask icon in the background of protected backpack slots.");

    public ConfigEntry<Color> ProtectedItemStolenColor = config.Bind<Color>("SmugglersPack", "StolenIconTint", new Color(0f, 1f, 1f, 0.6f), "Color to tint the red handcuff icon when an item is in a protected backpack slot.");
}
