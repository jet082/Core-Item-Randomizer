using HarmonyLib;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Handlers;
using UnityEngine;
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
					bool doWeHaveThis = TechTypeHandler.TryGetModdedTechType("RandoSeamothDoll", out TechType outTechType);
					PrefabPlaceholdersGroup pre = __instance.gameObject.EnsureComponent<PrefabPlaceholdersGroup>();
					pre.prefabPlaceholders[0].prefabClassId = CraftData.GetClassIdForTechType(outTechType);
					//var boxContents = pre.prefabPlaceholders[0].gameObject;
					//boxContents.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
					//boxContents.EnsureComponent<Pickupable>();
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
			BaseBioReactor.charge.Remove(TechType.Hoverfish);
			/*SupplyCrate tempSupplyCrate = __instance as SupplyCrate;
			bool doWeHaveThis = TechTypeHandler.TryGetModdedTechType("RandoSeamothDoll", out TechType outTechType);
			var task = CraftData.GetPrefabForTechTypeAsync(outTechType);
			GameObject prefab = task.GetResult();
			PluginSetup.BepinExLogger.LogInfo(prefab.gameObject.name);
			if (prefab == null) return;
			GameObject obj = GameObject.Instantiate(prefab, tempSupplyCrate.transform.position, Random.rotation);
			var pickupObj = obj.EnsureComponent<Pickupable>();
			tempSupplyCrate.itemInside = pickupObj; */
			//LightmappedPrefabs.main.RequestScenePrefab("cyclops", new LightmappedPrefabs.OnPrefabLoaded(OnSubPrefabLoaded));
		}
	}
}