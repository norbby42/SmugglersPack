using BepInEx.Configuration;

namespace SmugglersPack.Settings;

public class SmugglersPackSettings(ConfigFile config)
{
    public ConfigEntry<bool> MySettingsBool = config.Bind<bool>("SectionName", "MySettingsBool", true, "This is an example boolean setting!");
}
