using HarmonyLib;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Handlers;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using UWE;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class BoxPatcher
	{
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
					bool doWeHaveThis = TechTypeHandler.TryGetModdedTechType("SeamothDoll", out TechType outTechType);
					PrefabPlaceholdersGroup pre = __instance.gameObject.EnsureComponent<PrefabPlaceholdersGroup>();
					pre.prefabPlaceholders[0].prefabClassId = CraftData.GetClassIdForTechType(outTechType);
					pre.prefabPlaceholders[0].highPriority = true;
					pre.prefabPlaceholders[0].name = outTechType.AsString();
				}
			}
		}
		[HarmonyPatch(typeof(SupplyCrate), nameof(SupplyCrate.FindInsideItemAfterStart))]
		[HarmonyPostfix]
		public static void PatchFindInside(SupplyCrate __instance)
		{
			for(int i = 0; i < __instance.gameObject.transform.childCount; i = i + 1)
			{
				var childEntry = __instance.gameObject.transform.GetChild(i);
				PluginSetup.BepinExLogger.LogInfo($"Woah it's Child ${i}. Name: {childEntry.name}, {childEntry}");
			}
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