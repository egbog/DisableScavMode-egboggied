using EFT.UI;
using System.Reflection;
using SPT.Reflection.Patching;
using UnityEngine;
using UnityEngine.UI;
using EFT;

namespace SIT.Core.SP.ScavMode
{
	public class DisableInsuranceScreenPatch : ModulePatch
	{
		protected override MethodBase GetTargetMethod()
		{
			return typeof(MainMenuControllerClass).GetMethod("method_79", BindingFlags.Public | BindingFlags.Instance);
		}

		[PatchPrefix]
		static bool PatchPrefix(MainMenuControllerClass __instance, RaidSettings ___raidSettings_0)
		{
			if (Plugin.DisableInsuranceScreen.Value)
				___raidSettings_0.RaidMode = ERaidMode.Local;

			return true;
		}
	}
}
