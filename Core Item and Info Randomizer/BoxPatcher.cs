using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class BoxPatcher
	{
		public static Vector3 BoxContentSize = new Vector3(0.21f, 0.15f, 0.13f);
		public static float[] boxContentsPadding = { 0.3f, 0.8f, 0.5f };

		public static HashSet<string> VFXAllowList = new() { "VFXSurface", "VFXFabricating", "VFXController" };
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

					string toCommit = "RandoSeamothDoll";
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
						outTechType = TechType.StasisRifle;
						prefabClassIdToCommit = CraftData.GetClassIdForTechType(outTechType);
					}

					pre.prefabPlaceholders[0].prefabClassId = prefabClassIdToCommit;

					GameObject prefabGameObject = pre.prefabPlaceholders[0].gameObject;
					if (prefabClassIdToCommit == ModCache.CacheData["RandoCyclopsDoll"].ClassId)
					{
						prefabGameObject.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f);
					}
					else
					{
						CoroutineHost.StartCoroutine(ResizeToBox(prefabGameObject, prefabClassIdToCommit, __instance.gameObject));
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
		public static Bounds FindItemSize(GameObject someGameObject)
		{
			Quaternion currentRotation = someGameObject.transform.rotation;
			Vector3 currentScale = someGameObject.transform.localScale;
			someGameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			someGameObject.transform.localScale = Vector3.one;
			bool stillHasComponents = true;
			while (stillHasComponents)
			{
				stillHasComponents = false;
				foreach (var someComponent in someGameObject.GetAllComponentsInChildren<Component>())
				{
					if (someComponent.GetType().ToString().Contains("VFX") && !VFXAllowList.Contains(someComponent.GetType().ToString()))
					{
						Renderer[] arrayOfVFXRenderers = someComponent.GetAllComponentsInChildren<Renderer>();
						foreach (var someRenderer in arrayOfVFXRenderers)
						{
							GameObject.DestroyImmediate(someComponent);
							GameObject.DestroyImmediate(someRenderer);
						}
					}
				}
			}
			Renderer[] rendererArray = someGameObject.GetAllComponentsInChildren<Renderer>();
			Bounds bounds = new Bounds(someGameObject.transform.position, Vector3.zero);
			bool hasTriggeredFoundBounds = false;
			foreach (Renderer renderer in rendererArray)
			{
				if (renderer.enabled == true && renderer.gameObject.activeSelf && !(renderer.bounds.size.x == 0 && renderer.bounds.size.y == 0 && renderer.bounds.size.z == 0))
				{
					if (!hasTriggeredFoundBounds)
					{
						bounds = renderer.bounds;
						hasTriggeredFoundBounds = true;
					}
					PluginSetup.BepinExLogger.LogInfo($"Collider Size Check: {renderer.bounds.size.x}, {renderer.bounds.size.y}, {renderer.bounds.size.z} at center {renderer.bounds.center.x}, {renderer.bounds.center.y}, {renderer.bounds.center.z}: {renderer.gameObject.name}");
					bounds.Encapsulate(renderer.bounds);
				}
			}
			Vector3 localCenter = bounds.center - someGameObject.transform.position;
			bounds.center = localCenter;
			someGameObject.transform.rotation = currentRotation;
			PluginSetup.BepinExLogger.LogInfo($"Bounds Size Check: {bounds.size.x}, {bounds.size.y}, {bounds.size.z}, at center {bounds.center.x}, {bounds.center.y}, {bounds.center.z}");
			return bounds;
		}
		public static IEnumerator ResizeToBox(GameObject someGameObject, string someClassId, GameObject supplyBox2)
		{
			IPrefabRequest boxTask = PrefabDatabase.GetPrefabAsync("580154dd-b2a3-4da1-be14-9a22e20385c8");
			yield return boxTask;
			_ = boxTask.TryGetPrefab(out GameObject prefabBox);
			GameObject supplyBox = GameObject.Instantiate(prefabBox);
			//Try to get box size...
			Bounds boxBounds = FindItemSize(supplyBox);

			//Start with prefab. Need to instantiate to get proper collider values.
			IPrefabRequest task = PrefabDatabase.GetPrefabAsync(someClassId);
			yield return task;
			_ = task.TryGetPrefab(out GameObject prefab);
			GameObject prefabObject = GameObject.Instantiate(prefab);

			//Now let's try to get the prefab size...
			Bounds bounds = FindItemSize(prefabObject);

			//Find the smallest scaling factor to fit in the box.
			float minScalingFactor;

			if (boxBounds.size.x - boxContentsPadding[0] < bounds.size.x || boxBounds.size.y - boxContentsPadding[1] < bounds.size.y || boxBounds.size.z - boxContentsPadding[2] < bounds.size.z)
			{
				minScalingFactor = float.PositiveInfinity;
				for (int i = 0; i <= 2; i = i + 1)
				{
					float potentialScalingFactor = (boxBounds.size[i] - boxContentsPadding[i]) / bounds.size[i];
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
			//Delete the duplicate lest it appear out in the wild.
			GameObject.DestroyImmediate(prefabObject);
			GameObject.DestroyImmediate(supplyBox);
		}
	}
}