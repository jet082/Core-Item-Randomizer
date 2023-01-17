using HarmonyLib;
using SMLHelper.V2.Handlers;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class BoxPatcher
	{
		public static Dictionary<string, Vector3> TechTypeScaleTranslation = new()
		{
			{"RandoSeamothDoll", new Vector3(5.1023483091f, 5.1023483091f, 5.1023483091f)},
			{"RandoPrawnSuitDoll", new Vector3(4.1023483091f, 4.1023483091f, 4.1023483091f)},
			{"RandoCyclopsDoll", new Vector3(0.1023483091f, 0.1023483091f, 0.1023483091f)}
		};
		[HarmonyPatch(typeof(HandTarget))]
		[HarmonyPatch(nameof(HandTarget.Awake))]
		[HarmonyPostfix]
		public static void PatchHandTarget(HandTarget __instance)
		{
			if (__instance is SupplyCrate)
			{
				PluginSetup.BepinExLogger.LogInfo($"Coordinate of Crate is {__instance.transform.position}");
				if (__instance.transform.position.Equals(new Vector3(0f, 0f, 0f)))
				{
					__instance.gameObject.EnsureComponent<Sealed>()._sealed = true;
					__instance.gameObject.EnsureComponent<ImmuneToPropulsioncannon>().immuneToRepulsionCannon = true;

					//This is how we get items in boxes.
					PrefabPlaceholdersGroup pre = __instance.gameObject.EnsureComponent<PrefabPlaceholdersGroup>();
					
					_ = TechTypeHandler.TryGetModdedTechType("RandoCyclopsDoll", out TechType outTechType);
					string prefabClassIdToCommit = CraftData.GetClassIdForTechType(outTechType);
					pre.prefabPlaceholders[0].prefabClassId = prefabClassIdToCommit;

					//We need to do this for any custom items or else they won't show up in the box...
					WorldEntityInfo worldInfoData = new WorldEntityInfo();
					worldInfoData.classId = prefabClassIdToCommit;
					worldInfoData.cellLevel = LargeWorldEntity.CellLevel.Near;
					worldInfoData.techType = outTechType;
					WorldEntityDatabase.main.infos.Add(prefabClassIdToCommit, worldInfoData);
					
					GameObject prefabGameObject = pre.prefabPlaceholders[0].gameObject;
					if (TechTypeScaleTranslation.ContainsKey(prefabClassIdToCommit))
					{
						prefabGameObject.transform.localScale = TechTypeScaleTranslation[prefabClassIdToCommit];
					}
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