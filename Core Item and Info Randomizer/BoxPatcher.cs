using HarmonyLib;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using UWE;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class BoxPatcher
	{
		public static float scaler = 0.1023483091f;
		[HarmonyPatch(typeof(Vehicle))]
		[HarmonyPatch(nameof(Vehicle.OnHandHover))]
		[HarmonyPrefix]
		public static bool PatchInteriorVehiclesHover(Vehicle __instance)
		{
			// Make sure that you can't enter the Seamoth/Prawn Suit while it's tiny in a chest.
			if ((__instance is SeaMoth or Exosuit) && __instance.gameObject.transform.localScale == new Vector3(scaler, scaler, scaler) && __instance.GetComponentInParent<SupplyCrate>() != null)
			{
				return false;
			} else
			{
				return true;
			}
		}
		[HarmonyPatch(typeof(Vehicle))]
		[HarmonyPatch(nameof(Vehicle.OnHandClick))]
		[HarmonyPrefix]
		public static bool PatchInteriorVehiclesClick(Vehicle __instance)
		{
			// Make sure that you can't enter the Seamoth/Prawn Suit while it's tiny in a chest.
			if ((__instance is SeaMoth or Exosuit) && __instance.gameObject.transform.localScale == new Vector3(scaler, scaler, scaler) && __instance.GetComponentInParent<SupplyCrate>() != null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
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
					PrefabPlaceholdersGroup pre = __instance.gameObject.EnsureComponent<PrefabPlaceholdersGroup>();
					pre.prefabPlaceholders[0].prefabClassId = CraftData.GetClassIdForTechType(TechType.RocketBase);
					/*bool doWeHaveThis = TechTypeHandler.TryGetModdedTechType("Kit_BaseObservatory", out TechType outTechType);
					pre.prefabPlaceholders[0].prefabClassId = CraftData.GetClassIdForTechType(outTechType);
					pre.prefabPlaceholders[0].highPriority = true;
					pre.prefabPlaceholders[0].name = outTechType.AsString();*/

					GameObject prefabGameObject = pre.prefabPlaceholders[0].gameObject;
					prefabGameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
					//prefabGameObject.transform.localScale = new Vector3(scaler, scaler, scaler);
					prefabGameObject.EnsureComponent<HandTarget>().isValidHandTarget = false;
				}
			}
		}
		[HarmonyPatch(typeof(SupplyCrate), nameof(SupplyCrate.FindInsideItemAfterStart))]
		[HarmonyPostfix]
		public static void PatchFindInside(SupplyCrate __instance)
		{
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
		private static void OnSubPrefabLoaded(GameObject prefab)
		{
			//This just summons the Cyclops. It'll probably be moved or removed, but for now it's just nice to be able to trigger it by opening a chest.
			Vector3 playerPosition = Player.main.transform.position;
			var spawnPosition = playerPosition + new Vector3(0f, 25f, 5f);
			GameObject gameObject = global::Utils.SpawnPrefabAt(prefab, null, spawnPosition);
			gameObject.transform.rotation = Quaternion.identity;
			gameObject.SetActive(true);
			gameObject.SendMessage("StartConstruction", SendMessageOptions.DontRequireReceiver);
			LargeWorldEntity.Register(gameObject);
			CrafterLogic.NotifyCraftEnd(gameObject, CraftData.GetTechType(gameObject));
		}
		[HarmonyPatch(typeof(SupplyCrate), nameof(SupplyCrate.ToggleOpenState))]
		[HarmonyPostfix]
		public static void PatchOpenBox(SupplyCrate __instance)
		{
			//LightmappedPrefabs.main.RequestScenePrefab("cyclops", new LightmappedPrefabs.OnPrefabLoaded(OnSubPrefabLoaded));
		}
	}
}