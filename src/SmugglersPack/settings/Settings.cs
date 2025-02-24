using BepInEx.Configuration;
using UnityEngine;

namespace SmugglersPack.Settings;

public class SmugglersPackSettings(ConfigFile config)
{
    public ConfigEntry<int> NumberHiddenSlots = config.Bind<int>("SmugglersPack", "NumberHiddenSlots", 8, "The number of hidden slots in the backpack.  Valid range: (1, 24)");

    public ConfigEntry<Color> HiddenSlotOutline = config.Bind<Color>("SmugglersPack", "HiddenSlotOutlineColor", new Color(0f, 0.9f, 0f), "The RGB color to outline hidden slots in the UI");
}
