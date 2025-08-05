using _DisableScavMode_egboggied.Patches;
using BepInEx;
using BepInEx.Configuration;

namespace _DisableScavMode_egboggied;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInProcess("EscapeFromTarkov.exe")]
public class Plugin : BaseUnityPlugin {
    public static ConfigEntry<bool> DisableInsuranceScreen { get; set; }

    private void Awake() {
        InitConfig();
        new DisableScavModePatch().Enable();
        new DisableInsuranceScreenPatch().Enable();
    }

    private void InitConfig() {
        const string insuranceScreen = "Disable Insurance Screen";

        DisableInsuranceScreen = Config.Bind(insuranceScreen, "Skips insurance screen if enabled.", true);
    }
}