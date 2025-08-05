using System.Reflection;
using EFT;
using SPT.Reflection.Patching;

namespace _DisableScavMode_egboggied.Patches;

public class DisableInsuranceScreenPatch : ModulePatch {
    protected override MethodBase GetTargetMethod() {
        return typeof(MainMenuControllerClass).GetMethod("method_79", BindingFlags.Public | BindingFlags.Instance);
    }

    [PatchPrefix]
    public static bool Prefix(MainMenuControllerClass __instance, RaidSettings ___raidSettings_0) {
        if (Plugin.DisableInsuranceScreen.Value) ___raidSettings_0.RaidMode = ERaidMode.Local;

        return true;
    }
}