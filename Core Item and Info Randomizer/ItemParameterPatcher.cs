using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch(typeof(Vehicle))]
	public class ItemParameterPatcher
	{
		[HarmonyPatch(nameof(Vehicle.Awake))]
		[HarmonyPostfix]
		public static void PatchVehicle(Vehicle __instance)
		{
			if (__instance.GetType() == typeof(SupplyCrate))
			{
				Vehicle someVehicle = __instance as SeaMoth;
			}
		}
	}
}
