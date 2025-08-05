using System.Reflection;
using EFT.UI.Matchmaker;
using SPT.Reflection.Patching;
using UnityEngine;
using UnityEngine.UI;

namespace _DisableScavMode_egboggied.Patches;

public class DisableScavModePatch : ModulePatch {
    protected override MethodBase GetTargetMethod() {
        return typeof(MatchMakerSideSelectionScreen).GetMethod("Awake", BindingFlags.Public | BindingFlags.Instance);
    }

    [PatchPostfix]
    public static void Postfix(MatchMakerSideSelectionScreen __instance, Button ____savagesBigButton,
                               Button                        ____pmcBigButton) {
        ____savagesBigButton.transform.parent.gameObject.SetActive(false);
        ____pmcBigButton.transform.parent.transform.localPosition = new Vector3(-220, 520, 0);
    }
}