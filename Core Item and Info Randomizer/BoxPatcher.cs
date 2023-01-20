using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class BoxPatcher
	{
		public static Dictionary<string, Tuple<float, bool>> TechTypeScaleTranslation = new()
		{
			{ "RandoSeamothDoll", new Tuple<float, bool>(0.18f, false) },
			{ "RandoPrawnSuitDoll", new Tuple<float, bool>(0.15f, false) },
			{ "RandoCyclopsDoll", new Tuple<float, bool>(0.012f, false) },
			{ "RandoRocketBaseDoll", new Tuple<float, bool>(0.007f, false) },
			{ CraftData.GetClassIdForTechType(TechType.ReaperLeviathan), new Tuple<float, bool>(0.05f, true) }
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

					string toCommit = "RandoCyclopsDoll";
					TechType outTechType;
					string prefabClassIdToCommit;
					if (ModCache.CacheData.ContainsKey(toCommit))
					{
						outTechType = ModCache.CacheData[toCommit].ModTechType;
						prefabClassIdToCommit = ModCache.CacheData[toCommit].ClassId;
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
						float scaler = TechTypeScaleTranslation[prefabClassIdToCommit].Item1;
						if (TechTypeScaleTranslation[prefabClassIdToCommit].Item2)
						{
							prefabGameObject.GetComponentInParent<Creature>().SetScale(scaler);
						} else
						{
							prefabGameObject.transform.localScale = new Vector3(scaler, scaler, scaler);
						}
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