using HarmonyLib;
using SMLHelper.V2.Handlers;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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
		public static TechType[] Resizables = { TechType.Seamoth, TechType.Exosuit, TechType.Cyclops, TechType.RocketBase };
		[HarmonyPatch(typeof(SupplyCrate))]
		[HarmonyPatch(nameof(SupplyCrate.OnHandClick))]
		[HarmonyPrefix]
		public static bool StopAddedItemIcon(SupplyCrate __instance)
		{
			string techName = __instance.itemInside.GetTechName();
			//This just removes the addition of the Cyclops Doll to the inventory. It has to be done here to stop the icon from showing up.
			if (__instance.open && DollToVehicleTranslation.ContainsKey(techName))
			{
				Pickupable itemInside = __instance.itemInside;
				_ = TechTypeHandler.TryGetModdedTechType(techName, out TechType outTechType);
				if (__instance.itemInside.GetTechType() == outTechType)
				{
					GameObject.Destroy(itemInside.gameObject);
					CraftData.AddToInventory(DollToVehicleTranslation[techName]);
					return false;
				}
			}
			return true;
		}
		[HarmonyPatch(typeof(Pickupable))]
		[HarmonyPatch(nameof(Pickupable.Pickup))]
		[HarmonyPrefix]
		public static void PatchVehiclePickup(Pickupable __instance)
		{
			if (Resizables.Contains(__instance.GetTechType())) {
				__instance.transform.localScale = new Vector3(1f, 1f, 1f);
			}
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