using HarmonyLib;

namespace CoreItemRandomizer
{
	[HarmonyPatch(typeof(Survival))]
	public class InfiniteSurvival
	{
		[HarmonyPatch(nameof(Survival.UpdateStats))]
		[HarmonyPostfix]
		public static void PatchSurvival(Survival __instance)
		{
			__instance.food = 100f;
			__instance.water = 100f;
		}
	}
}
