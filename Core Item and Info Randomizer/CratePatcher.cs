using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class CratePatcher
	{
		[HarmonyPatch(typeof(HandTarget))]
		[HarmonyPatch(nameof(HandTarget.Awake))]
		[HarmonyPrefix]
		public static void PatchHandTarget(HandTarget __instance)
		{
			if (__instance is SupplyCrate)
			{
				Vector3 crateCoordinates = __instance.transform.position;
				if (__instance.transform.position.Equals(new Vector3(0f, 0f, 0f)))
				{
					CrateContents boxContentsSettings = __instance.gameObject.EnsureComponent<CrateContents>();
					boxContentsSettings.boxContentsModItem = "";
					boxContentsSettings.boxContentsTechType = TechType.HatchingEnzymes;
					boxContentsSettings.isSealed = true;
					boxContentsSettings.PlaceScaledItemInside();
				}
			}
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