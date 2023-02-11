using UnityEngine;
using System.Collections.Generic;
using HarmonyLib;
using System.Linq;

namespace CoreItemRandomizer.Misc
{
	[HarmonyPatch]
	internal class RandomStartingLocation
	{
		[HarmonyPatch(typeof(EscapePod), nameof(EscapePod.FixedUpdate))]
		[HarmonyPostfix]
		public static void KeepItStill(EscapePod __instance)
		{
			if (PluginSetup.RandomStartingLocation && __instance.transform.position.y < 0)
			{
				WorldForces wf = __instance.GetComponent<WorldForces>();
				wf.underwaterGravity = 0.0f;
				__instance.rigidbodyComponent.isKinematic = true;
			}
		}
		[HarmonyPatch(typeof(RandomStart), nameof(RandomStart.GetRandomStartPoint))]
		[HarmonyPostfix]
		public static void RandomizeStartingLocation(ref Vector3 __result)
		{
			if (PluginSetup.RandomStartingLocation)
			{
				__result = PluginSetup.CachedRandoData.StartingLocation;
			}
		}
	}
}