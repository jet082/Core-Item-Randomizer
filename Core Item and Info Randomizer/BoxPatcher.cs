using HarmonyLib;
using System.Collections;
using UnityEngine;
using UWE;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class BoxPatcher
	{
		public static Vector3 BoxContentSize = new Vector3(0.51f, 0.56f, 1.55f);
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

					string toCommit = "RandoSeamothDolla";
					TechType outTechType;
					string prefabClassIdToCommit;
					//We need to do this for any custom items or else they won't show up in the box...
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
						outTechType = TechType.PropulsionCannon;
						prefabClassIdToCommit = CraftData.GetClassIdForTechType(outTechType);
					}

					pre.prefabPlaceholders[0].prefabClassId = prefabClassIdToCommit;
					
					GameObject prefabGameObject = pre.prefabPlaceholders[0].gameObject;
					if (prefabClassIdToCommit == ModCache.CacheData["RandoCyclopsDoll"].ClassId)
					{
						pre.prefabPlaceholders[0].transform.localScale = new Vector3(0.012f, 0.012f, 0.012f);
					}
					else
					{
						CoroutineHost.StartCoroutine(ResizeToBox(prefabGameObject, prefabClassIdToCommit));
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
		public static IEnumerator ResizeToBox(GameObject someGameObject, string someClassId)
		{
			IPrefabRequest task = PrefabDatabase.GetPrefabAsync(someClassId);
			yield return task;
			_ = task.TryGetPrefab(out GameObject prefab);
			Collider[] colliders = prefab.GetAllComponentsInChildren<Collider>();
			UnityEngine.Bounds bounds = new UnityEngine.Bounds(Vector3.zero, Vector3.zero);
			foreach (Collider someCollider in colliders)
			{
				PluginSetup.BepinExLogger.LogInfo($"Collider Size Check: {someCollider.bounds.size.x}, {someCollider.bounds.size.y}, {someCollider.bounds.size.z}");
				if (someCollider.isTrigger == false && someCollider.enabled == true)
				{
					bounds.Encapsulate(someCollider.bounds);
				}
			}
			PluginSetup.BepinExLogger.LogInfo($"Bound Size Check: {bounds.size.x}, {bounds.size.y}, {bounds.size.z}");
			float minScalingFactor;

			if (BoxContentSize.x < bounds.size.x || BoxContentSize.y < bounds.size.y || BoxContentSize.z < bounds.size.z)
			{
				minScalingFactor = float.PositiveInfinity;
				for (int i = 0; i <= 2; i = i + 1)
				{
					float potentialScalingFactor = BoxContentSize[i] / bounds.size[i];
					if (potentialScalingFactor < minScalingFactor)
					{
						minScalingFactor = potentialScalingFactor;
					}
				}
			}
			else
			{
				minScalingFactor = 1f;
			}
			Vector3 scaler = new Vector3(minScalingFactor, minScalingFactor, minScalingFactor);
			someGameObject.transform.localScale = scaler;
		}
	}
}