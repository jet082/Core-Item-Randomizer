using HarmonyLib;

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
			SaveData saveData = PluginSetup.RandomizerLoadedSaveData;
			if (__instance is SupplyCrate && !__instance.gameObject.GetComponent<CrateContents>() && saveData.ChestPlacementData.ContainsKey(__instance.transform.position.ToString()))
			{
				CrateContents boxContentsSettings = __instance.gameObject.EnsureComponent<CrateContents>();
				boxContentsSettings.boxContentsClassId = saveData.ChestPlacementData[__instance.transform.position.ToString()][0];
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