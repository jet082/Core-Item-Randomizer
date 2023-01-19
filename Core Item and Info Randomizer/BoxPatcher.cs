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
			{"RandoSeamothDoll", new Vector3(0.181023483091f, 0.181023483091f, 0.181023483091f)},
			{"RandoPrawnSuitDoll", new Vector3(2.1023483091f, 2.1023483091f, 2.1023483091f)},
			{"RandoCyclopsDoll", new Vector3(0.1023483091f, 0.1023483091f, 0.1023483091f)},
			{CraftData.GetClassIdForTechType(TechType.RocketBase), new Vector3(0.0071023483091f, 0.0071023483091f, 0.0071023483091f)},
			{CraftData.GetClassIdForTechType(TechType.ReaperLeviathan), new Vector3(0.05f, 0.05f, 0.05f) }
		};
		public static Dictionary<string, TechType> CustomItems = new()
		{
			{"RandoSeamothDoll", TechType.Seamoth},
			{"RandoPrawnSuitDoll", TechType.Exosuit},
			{"RandoCyclopsDoll", TechType.Cyclops},
			{"Kit_BaseObservatory", TechType.BaseObservatory}
		};
		[HarmonyPatch(typeof(HandTarget))]
		[HarmonyPatch(nameof(HandTarget.Awake))]
		[HarmonyPostfix]
		public static void PatchHandTarget(HandTarget __instance)
		{
			if (__instance is SupplyCrate)
			{
				Vector3 crateCoordinates = __instance.transform.position;
				PluginSetup.BepinExLogger.LogInfo($"Coordinate of Crate is {crateCoordinates}");
				if (__instance.transform.position.Equals(new Vector3(0f, 0f, 0f)))
				{
					__instance.gameObject.EnsureComponent<Sealed>()._sealed = true;
					__instance.gameObject.EnsureComponent<ImmuneToPropulsioncannon>().immuneToRepulsionCannon = true;

					//This is how we get items in boxes.
					PrefabPlaceholdersGroup pre = __instance.gameObject.EnsureComponent<PrefabPlaceholdersGroup>();

					var toCommit = "RandoSeamothDoll";
					TechType outTechType;
					string prefabClassIdToCommit;
					if (CustomItems.ContainsKey(toCommit))
					{
						_ = TechTypeHandler.TryGetModdedTechType(toCommit, out outTechType);
						prefabClassIdToCommit = CraftData.GetClassIdForTechType(outTechType);
						WorldEntityInfo worldInfoData = new WorldEntityInfo();
						worldInfoData.classId = prefabClassIdToCommit;
						worldInfoData.cellLevel = LargeWorldEntity.CellLevel.Far;
						worldInfoData.techType = outTechType;
						WorldEntityDatabase.main.infos.Add(prefabClassIdToCommit, worldInfoData);
					} else
					{
						outTechType = TechType.RocketBase;
						prefabClassIdToCommit = CraftData.GetClassIdForTechType(outTechType);
					}

					pre.prefabPlaceholders[0].prefabClassId = prefabClassIdToCommit;

					//We need to do this for any custom items or else they won't show up in the box...
					
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