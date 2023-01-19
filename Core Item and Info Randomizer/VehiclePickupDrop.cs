using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using SMLHelper.V2.Handlers;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class VehiclePickupDrop
	{
		public static Dictionary<string, TechType> DollToVehicleTranslation = new()
		{
			{"RandoSeamothDoll", TechType.Seamoth},
			{"RandoPrawnSuitDoll", TechType.Exosuit},
			{"RandoCyclopsDoll", TechType.Cyclops}
		};
		public static HashSet<TechType> Resizables = new() { TechType.Seamoth, TechType.Exosuit, TechType.Cyclops, TechType.RocketBase };
		[HarmonyPatch(typeof(SupplyCrate))]
		[HarmonyPatch(nameof(SupplyCrate.OnHandClick))]
		[HarmonyPrefix]
		public static bool PatchSupplyCrateClick(SupplyCrate __instance)
		{
			return StopClick(__instance.itemInside.gameObject, __instance.itemInside.GetTechName(), __instance.itemInside.GetTechType());
		}

		[HarmonyPatch(typeof(Pickupable))]
		[HarmonyPatch(nameof(Pickupable.OnHandClick))]
		[HarmonyPrefix]
		public static bool PatchPickupableClick(Pickupable __instance)
		{
			return StopClick(__instance.gameObject, __instance.GetTechName(), __instance.GetTechType());
		}
		[HarmonyPatch(typeof(Pickupable))]
		[HarmonyPatch(nameof(Pickupable.Drop), new[] { typeof(Vector3), typeof(Vector3), typeof(bool) })]
		[HarmonyPostfix]
		public static void PatchVehicleDrop(Pickupable __instance)
		{
			if (__instance.GetTechType() == TechType.Cyclops)
			{
				if (!PluginSetup.RandomizerLoadedSaveData.HasLoadedCyclopsYet)
				{
					PluginSetup.RandomizerLoadedSaveData.HasLoadedCyclopsYet = true;
					GameObject.Destroy(__instance);
					LightmappedPrefabs.main.RequestScenePrefab("cyclops", new LightmappedPrefabs.OnPrefabLoaded(OnSubPrefabLoaded));
				}
			}
		}
		[HarmonyPatch(typeof(Exosuit))]
		[HarmonyPatch(nameof(Exosuit.Awake))]
		[HarmonyPostfix]
		public static void CleanExosuit(Exosuit __instance)
		{
			_ = TechTypeHandler.TryGetModdedTechType("RandoPrawnSuitDoll", out TechType outTechType);
			if (__instance.name == "RandoPrawnSuitDoll")
			{
				GameObject.Destroy(__instance);
			}
		}
		private static bool StopClick(GameObject someObject, string someString, TechType someTechType)
		{
			if (DollToVehicleTranslation.ContainsKey(someString))
			{
				GameObject.Destroy(someObject);
				CraftData.AddToInventory(DollToVehicleTranslation[someString]);
				return false;
			}
			if (Resizables.Contains(someTechType))
			{
				someObject.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			return true;
		}
		private static void OnSubPrefabLoaded(GameObject prefab)
		{
			//This just summons the Cyclops. Temporarily using the coordinates of the test box.
			Vector3 spawnPosition = MainCamera.camera.transform.position + (15f * MainCamera.camera.transform.forward);
			GameObject gameObject = global::Utils.SpawnPrefabAt(prefab, null, spawnPosition);
			gameObject.transform.rotation = Quaternion.identity;
			gameObject.SetActive(true);
			gameObject.SendMessage("StartConstruction", SendMessageOptions.DontRequireReceiver);
			LargeWorldEntity.Register(gameObject);
			CrafterLogic.NotifyCraftEnd(gameObject, TechType.Cyclops);
		}
	}
}