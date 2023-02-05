using System.Collections.Generic;
using UnityEngine;
using UWE;
using System.Collections;
using HarmonyLib;

namespace CoreItemAndInfoRandomizer
{
	public class CrateContents : MonoBehaviour
	{
		public string boxContentsClassId = "";
		public bool isSealed;
		private static readonly float[] boxContentsPadding = { 0.3f, 0.8f, 0.5f };
		private static readonly HashSet<string> VFXAllowList = new() { "VFXSurface", "VFXFabricating", "VFXController" };
		private static readonly HashSet<string> FiddlyIgnoreList = new() { "Mesh", "x_flashlightCone", "beamLeft", "beamRight", "x_MapRoom_HoloTableGlow_Bottom", "x_Center", "xMapRoomDust", "Ping", "Line", "x_CameraSeaGlide" };
		public static readonly Dictionary<string, Vector3> FiddlySpecialCases = new () {
			{ ModCache.CacheData["RandoCyclopsDoll"].ClassId, new Vector3(0.012f, 0.012f, 0.012f) },
			{ CraftData.GetClassIdForTechType(TechType.PrecursorIonCrystal), new Vector3(0.7f, 0.7f, 0.7f) },
			{ CraftData.GetClassIdForTechType(TechType.MapRoomCamera), new Vector3(0.65f, 0.65f, 0.65f) },
			{ CraftData.GetClassIdForTechType(TechType.HighCapacityTank), new Vector3(0.9f, 0.9f, 0.9f) },
			{ CraftData.GetClassIdForTechType(TechType.Tank), new Vector3(0.9f, 0.9f, 0.9f) },
			{ CraftData.GetClassIdForTechType(TechType.DoubleTank), new Vector3(0.9f, 0.9f, 0.9f) },
			{ CraftData.GetClassIdForTechType(TechType.PlasteelTank), new Vector3(0.9f, 0.9f, 0.9f) },
			{ CraftData.GetClassIdForTechType(TechType.CyclopsDecoy), new Vector3(0.2f, 0.2f, 0.2f) },
			{ CraftData.GetClassIdForTechType(TechType.PipeSurfaceFloater), new Vector3(0.15f, 0.15f, 0.15f) },
		};
		public void PlaceScaledItemInside()
		{
			Vector3 crateCoordinates = gameObject.transform.position;
			PluginSetup.BepinExLogger.LogInfo($"Coordinate of Crate is {crateCoordinates}");
			gameObject.EnsureComponent<ImmuneToPropulsioncannon>().immuneToRepulsionCannon = true;
			if (isSealed)
			{
				gameObject.EnsureComponent<Sealed>()._sealed = true;
			}
			//This is how we get custom items into boxes.
			if (!WorldEntityDatabase.main.infos.ContainsKey(boxContentsClassId))
			{
				WorldEntityInfo worldInfoData = new()
				{
					classId = boxContentsClassId,
					cellLevel = LargeWorldEntity.CellLevel.Far
				};
				WorldEntityDatabase.main.infos.Add(boxContentsClassId, worldInfoData);
			}
			PrefabPlaceholdersGroup pre = gameObject.GetComponent<PrefabPlaceholdersGroup>();
			pre.prefabPlaceholders[0].prefabClassId = boxContentsClassId;

			GameObject prefabGameObject = pre.prefabPlaceholders[0].gameObject;

			//The cyclops doll needs special treatment since it's a scene or something. There's also some weird cases.
			if (FiddlySpecialCases.ContainsKey(boxContentsClassId))
			{
				prefabGameObject.transform.localScale = FiddlySpecialCases[boxContentsClassId];
			}
			else
			{
				CoroutineHost.StartCoroutine(ResizeToBox(prefabGameObject, boxContentsClassId));
			}
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
			Bounds bounds = new(someGameObject.transform.position, Vector3.zero);
			bool hasTriggeredFoundBounds = false;
			foreach (Renderer renderer in rendererArray)
			{
				if (renderer.enabled == true && renderer.gameObject.activeSelf && !(renderer.bounds.size.x == 0 && renderer.bounds.size.y == 0 && renderer.bounds.size.z == 0) && !FiddlyIgnoreList.Contains(renderer.gameObject.name))
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
			someGameObject.transform.localScale = currentScale;
			someGameObject.transform.rotation = currentRotation;
			PluginSetup.BepinExLogger.LogInfo($"Bounds Size Check: {bounds.size.x}, {bounds.size.y}, {bounds.size.z}, at center {bounds.center.x}, {bounds.center.y}, {bounds.center.z}");
			return bounds;
		}
		public static IEnumerator ResizeToBox(GameObject someGameObject, string someClassId)
		{
			IPrefabRequest boxTask = PrefabDatabase.GetPrefabAsync(ModCache.CacheData["MyVeryOwnSupplyCrate"].ClassId);
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
				for (int i = 0; i <= 2; i++)
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
			Vector3 scaler = new(minScalingFactor, minScalingFactor, minScalingFactor);
			someGameObject.transform.localScale = scaler;
			//Delete the duplicate lest it appear out in the wild.
			GameObject.DestroyImmediate(prefabObject);
			GameObject.DestroyImmediate(supplyBox);
		}
	}
}
