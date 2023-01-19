using HarmonyLib;
using System;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class RandomFishSize
	{
		[HarmonyPatch(typeof(Creature))]
		[HarmonyPatch(nameof(Creature.Start))]
		[HarmonyPrefix]
		public static void RandomizeTheFish(Creature __instance)
		{
			UnityEngine.Random.InitState(DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond);
			float scaleFactor = UnityEngine.Random.Range(1f, 3f);
			if (UnityEngine.Random.value > .5)
			{
				__instance.SetScale(1 / scaleFactor);
			}
			else
			{
				__instance.SetScale(scaleFactor);
			}
		}
	}
}
