using HarmonyLib;
using System.Collections;
using UnityEngine;
using UWE;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class BoxPatcher
	{
		public static Vector3 BoxContentSize = new Vector3(0.21f, 0.15f, 0.13f);
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
						outTechType = TechType.PropulsionCannon;
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
		public static IEnumerator ResizeToBox(GameObject someGameObject, string someClassId, GameObject supplyBox)
		{
			//Try to get box size...
			Quaternion boxCurrentRotation = supplyBox.transform.rotation;
			Vector3 boxCurrentScale = supplyBox.transform.localScale;
			supplyBox.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			supplyBox.transform.localScale = Vector3.one;
			Renderer[] boxRendererArray = supplyBox.GetAllComponentsInChildren<Renderer>();
			Bounds boxBounds = new Bounds(supplyBox.transform.position, Vector3.zero);
			bool BoxHasTriggeredFoundBounds = false;
			foreach (Renderer renderer in boxRendererArray)
			{
				if (renderer.enabled == true && renderer.gameObject.activeSelf)
				{
					if (!BoxHasTriggeredFoundBounds)
					{
						boxBounds = renderer.bounds;
						BoxHasTriggeredFoundBounds = true;
					}
					PluginSetup.BepinExLogger.LogInfo($"Box Collider Size Check: {renderer.bounds.size.x}, {renderer.bounds.size.y}, {renderer.bounds.size.z} at center {renderer.bounds.center.x}, {renderer.bounds.center.y}, {renderer.bounds.center.z}");
					boxBounds.Encapsulate(renderer.bounds);
				}
			}
			Vector3 boxLocalCenter = boxBounds.center - supplyBox.transform.position;
			boxBounds.center = boxLocalCenter;
			supplyBox.transform.localScale = boxCurrentScale;
			supplyBox.transform.rotation = boxCurrentRotation;

			PluginSetup.BepinExLogger.LogInfo($"Box Bounds Size Check: {boxBounds.size.x}, {boxBounds.size.y}, {boxBounds.size.z} at center {boxBounds.center.x}, {boxBounds.center.y}, {boxBounds.center.z}");

			//Start with prefab. Need to instantiate to get proper collider values.
			IPrefabRequest task = PrefabDatabase.GetPrefabAsync(someClassId);
			yield return task;
			_ = task.TryGetPrefab(out GameObject prefab);
			GameObject prefabObject = GameObject.Instantiate(prefab);

			//Now let's try to get the prefab size...
			Quaternion currentRotation = prefabObject.transform.rotation;
			Vector3 currentScale = prefabObject.transform.localScale;
			prefabObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			prefabObject.transform.localScale = Vector3.one;
			Renderer[] rendererArray = prefabObject.GetAllComponentsInChildren<Renderer>();
			Bounds bounds = new Bounds(prefabObject.transform.position, Vector3.zero);
			bool hasTriggeredFoundBounds = false;
			foreach (Renderer renderer in rendererArray)
			{
				if (renderer.enabled == true && renderer.gameObject.activeSelf)
				{
					if (!hasTriggeredFoundBounds)
					{
						bounds = renderer.bounds;
						hasTriggeredFoundBounds = true;
					}
					PluginSetup.BepinExLogger.LogInfo($"Collider Size Check: {renderer.bounds.size.x}, {renderer.bounds.size.y}, {renderer.bounds.size.z} at center {renderer.bounds.center.x}, {renderer.bounds.center.y}, {renderer.bounds.center.z}");
					bounds.Encapsulate(renderer.bounds);
				}
			}
			Vector3 localCenter = bounds.center - prefabObject.transform.position;
			bounds.center = localCenter;
			prefabObject.transform.rotation = currentRotation;
			PluginSetup.BepinExLogger.LogInfo($"Bounds Size Check: {bounds.size.x}, {bounds.size.y}, {bounds.size.z}");

			//Find the smallest scaling factor to fit in the box.
			float minScalingFactor;

			if (boxBounds.size.x < bounds.size.x || boxBounds.size.y < bounds.size.y || boxBounds.size.z < bounds.size.z)
			{
				minScalingFactor = float.PositiveInfinity;
				for (int i = 0; i <= 2; i = i + 1)
				{
					float potentialScalingFactor = boxBounds.size[i] / bounds.size[i];
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
		}
	}
}