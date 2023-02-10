using UnityEngine;
using System.Collections.Generic;
using HarmonyLib;
using System.Linq;

namespace CoreItemAndInfoRandomizer.Misc
{
	[HarmonyPatch]
	internal class RandomStartingLocation
	{
		public static readonly Dictionary<Vector3, string> StartingLocations = new()
		{
			{ new Vector3(-728.0f, -761f, -274.7f), "Lost River Skull" },
			{ new Vector3(-110.6f, 0f, -347.1f), "The Safe Shallows" },
			{ new Vector3(401.2f, 0f, -524.1f), "Crash Zone Trench" },
			{ new Vector3(1001.9f, 0, -1120.3f), "Crash Zone SE" }
		};
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
				if (PluginSetup.CachedRandoData.StartingLocation.Equals(Vector3.zero))
				{
					int selectionNumber = UnityEngine.Random.Range(0, StartingLocations.Count);
					PluginSetup.CachedRandoData.StartingLocation = StartingLocations.ElementAt(selectionNumber).Key;
					__result = StartingLocations.ElementAt(selectionNumber).Key;
					PluginSetup.SpoilerLogData.StartingLocation = StartingLocations.ElementAt(selectionNumber).Value;
				}
				else
				{
					__result = PluginSetup.CachedRandoData.StartingLocation;
				}
			}
		}
	}
}