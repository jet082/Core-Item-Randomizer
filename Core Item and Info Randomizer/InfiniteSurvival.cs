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
			__instance.food = 100f;
			__instance.water = 100f;
		}
	}
}
