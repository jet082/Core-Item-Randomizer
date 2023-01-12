using HarmonyLib;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch(typeof(Battery))]
	public class InfiniteBattery
	{
		[HarmonyPatch(nameof(Battery.GetChargeValueText))]
		[HarmonyPostfix]
		public static void BatteryPatcher(Battery __instance)
		{
			Battery someBattery = __instance as Battery;
			someBattery._charge = 100f;
			someBattery._capacity = 100f;
		}
	}
}