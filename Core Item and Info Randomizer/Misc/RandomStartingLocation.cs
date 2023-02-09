using UnityEngine;
using System.Collections.Generic;
using HarmonyLib;

namespace CoreItemAndInfoRandomizer.Misc
{
	[HarmonyPatch]
	internal class RandomStartingLocation
	{
		public static readonly Dictionary<Vector3, string> StartingLocations = new()
		{

		};
		[HarmonyPatch(typeof(RandomStart), nameof(RandomStart.GetRandomStartPoint))]
		[HarmonyPostfix]
		public static void RandomizeStartingLocation(ref Vector3 __result)
		{
			if (PluginSetup.RandomStartingLocation && PluginSetup.CachedRandoData.StartingLocation.Equals(Vector3.zero))
			{

			}
		}
	}
}
