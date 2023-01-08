using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace Core_Item_and_Info_Randomizer
{
	[HarmonyPatch(typeof(Vehicle))]
	public class ItemParameterPatcher
	{
		[HarmonyPatch(nameof(Vehicle.Awake))]
		[HarmonyPostfix]
		public static void Awake_Prefix(Vehicle __instance)
		{
			if (__instance.GetType() == typeof(SupplyCrate))
			{
				Vehicle someVehicle = __instance as SeaMoth;
			}
		}
	}
}
