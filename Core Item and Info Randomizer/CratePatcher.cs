using HarmonyLib;
using System;
using System.Linq;
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
			if (__instance is SupplyCrate && !__instance.gameObject.GetComponent<CrateContents>() && CratePlacementsData.BoxPlacements.Keys.Contains(__instance.transform.position))
			{
				string toAppendModItem;
				TechType toAppendTechType;
				CrateContents boxContentsSettings = __instance.gameObject.EnsureComponent<CrateContents>();
				if (__instance.transform.position.Equals(new Vector3(0f, 0f, 0f)))
				{
					toAppendModItem = "";
					toAppendTechType = TechType.MapRoomHUDChip;
					boxContentsSettings.isSealed = true;
				}
				else
				{
					toAppendModItem = "";
					UnityEngine.Random.InitState(DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond);
					int randomKeyIndex = UnityEngine.Random.Range(0, CratePlacementsData.DistributionTable.Count);
					toAppendTechType = CratePlacementsData.DistributionTable.ElementAt(randomKeyIndex).Key;
				}
				boxContentsSettings.boxContentsModItem = toAppendModItem;
				boxContentsSettings.boxContentsTechType = toAppendTechType;
				boxContentsSettings.PlaceScaledItemInside();
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