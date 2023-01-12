using HarmonyLib;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class BoxPatcher
	{
		[HarmonyPatch(typeof(HandTarget))]
		[HarmonyPatch(nameof(HandTarget.Awake))]
		[HarmonyPostfix]
		public static void PatchHandTarget(HandTarget __instance)
		{
			if (__instance is SupplyCrate someSupplyCrate)
			{
				PluginSetup.BepinExLogger.LogInfo($"Coordinate of Crate is {someSupplyCrate.transform.position}");
				if (someSupplyCrate.transform.position.Equals(new Vector3(0f, 0f, 0f)))
				{
					PrefabPlaceholdersGroup pre = someSupplyCrate.gameObject.EnsureComponent<PrefabPlaceholdersGroup>();
					someSupplyCrate.gameObject.EnsureComponent<Sealed>()._sealed = true;
					someSupplyCrate.gameObject.EnsureComponent<ImmuneToPropulsioncannon>().immuneToRepulsionCannon = true;
					pre.prefabPlaceholders[0].prefabClassId = CraftData.GetClassIdForTechType(TechType.Seamoth);
					var boxContents = pre.prefabPlaceholders[0].gameObject;
					boxContents.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
					boxContents.EnsureComponent<Pickupable>();
				}
			}
		}
		[HarmonyPatch(typeof(SupplyCrate), nameof(SupplyCrate.FindInsideItemAfterStart))]
		[HarmonyPostfix]
		public static void PatchFindInside(SupplyCrate __instance)
		{
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