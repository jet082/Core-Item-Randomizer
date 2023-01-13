using DecorationsMod.NewItems;
using HarmonyLib;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UnityEngine;

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
