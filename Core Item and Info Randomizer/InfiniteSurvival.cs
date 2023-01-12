using HarmonyLib;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch(typeof(Survival))]
	public class InfiniteSurvival
	{
		[HarmonyPatch(nameof(Survival.UpdateStats))]
		[HarmonyPostfix]
		public static void PatchSurvival(Survival __instance)
		{
			Survival someSurvival = __instance;
			someSurvival.food = 100f;
			someSurvival.water = 100f;
		}
	}
}
