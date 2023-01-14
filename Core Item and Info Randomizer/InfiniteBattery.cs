using HarmonyLib;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch(typeof(EnergyMixin))]
	public class InfiniteBattery
	{
		[HarmonyPatch(nameof(EnergyMixin.ModifyCharge))]
		[HarmonyPostfix]
		public static void BatteryPatcher(EnergyMixin __instance)
		{
			IBattery someBattery = __instance.GetBattery();
			someBattery.charge = someBattery.capacity;
		}
	}
}