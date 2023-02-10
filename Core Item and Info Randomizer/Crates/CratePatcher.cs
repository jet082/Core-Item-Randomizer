using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class CratePatcher
	{
		[HarmonyPatch(typeof(HandTarget), nameof(HandTarget.Awake))]
		[HarmonyPrefix]
		public static void PatchHandTarget(HandTarget __instance)
		{
			if (__instance is SupplyCrate && !__instance.gameObject.GetComponent<CrateContents>() && PluginSetup.CachedRandoData.ChestPlacements.ContainsKey(__instance.transform.position.ToString()))
			{
				//This *nonsense* is required to get around the duplication bug.
				HashSet<GameObject> toDestroyChests = new();
				Dictionary<string, int> foundChests = new();
				foreach (Collider someCollider in Physics.OverlapSphere(Camera.main.transform.position, 50f))
				{
					if (someCollider.gameObject.GetComponent<SupplyCrate>())
					{
						PluginSetup.BepinExLogger.LogInfo($"Working on {someCollider.gameObject.GetComponent<PrefabIdentifier>().Id}");
						if (someCollider.GetComponent<PrefabIdentifier>().Id == "")
						{
							if (foundChests.ContainsKey(someCollider.gameObject.GetComponent<PrefabIdentifier>().Id) && !foundChests.ContainsValue(someCollider.gameObject.GetInstanceID()))
							{
								toDestroyChests.Add(someCollider.gameObject);
							}
							else
							{
								if (!foundChests.ContainsKey(someCollider.gameObject.GetComponent<PrefabIdentifier>().Id))
								{
									foundChests.Add(someCollider.gameObject.GetComponent<PrefabIdentifier>().Id, someCollider.gameObject.GetInstanceID());
								}
							}
						}
					}
					foreach (GameObject someChest in toDestroyChests) {
						GameObject.Destroy(someChest.gameObject);
					}
				}
				//Okay the duplication bug is handled now. Moving on...
				CrateContents boxContentsSettings = __instance.gameObject.EnsureComponent<CrateContents>();
				boxContentsSettings.boxContentsClassId = PluginSetup.CachedRandoData.ChestPlacements[__instance.transform.position.ToString()][0];
				boxContentsSettings.PlaceScaledItemInside();
			}
		}
		[HarmonyPatch(typeof(SupplyCrate), nameof(SupplyCrate.Start))]
		[HarmonyPostfix]
		public static void DestroyTheErrantBattery(SupplyCrate __instance)
		{
			GameObject.DestroyImmediate(__instance.gameObject.transform.Find("Battery(Placeholder)").gameObject);
		}
		[HarmonyPatch(typeof(SupplyCrate), nameof(SupplyCrate.FindInsideItemAfterStart))]
		[HarmonyPostfix]
		public static void PatchFindInside(SupplyCrate __instance)
		{
			// This section adds a battery or power cell to the item in the box.

			// item not initialized; do nothing.
			if (!__instance.itemInside)
				return;

			// item doesn't need a battery; do nothing.
			if (__instance.itemInside.GetComponent<EnergyMixin>() == null)
				return;

			// add default battery
			var techType = CraftData.GetTechType(__instance.itemInside.gameObject);
			CrafterLogic.NotifyCraftEnd(__instance.itemInside.gameObject, techType);
		}
	}
}