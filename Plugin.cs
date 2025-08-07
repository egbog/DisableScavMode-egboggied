using _DisableScavMode_egboggied.Patches;
using BepInEx;
using BepInEx.Configuration;

namespace _DisableScavMode_egboggied;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInProcess("EscapeFromTarkov.exe")]
public class Plugin : BaseUnityPlugin {
    public static ConfigEntry<bool> InsuranceScreen { get; set; }
    public static ConfigEntry<bool> ScavMode { get; set; }

    private void Awake() {
        InitConfig();
        new DisableScavModePatch().Enable();
        new DisableInsuranceScreenPatch().Enable();
    }

    private void InitConfig() {
        const string insuranceScreen = "Insurance Screen";
        const string scavMode = "Scav Mode";

        InsuranceScreen = Config.Bind(insuranceScreen, "Is Insurance screen enabled?", true);
        ScavMode        = Config.Bind(scavMode,        "Is Scav mode enabled?", true);
    }
}