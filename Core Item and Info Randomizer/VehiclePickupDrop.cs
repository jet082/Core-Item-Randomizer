using HarmonyLib;
using UnityEngine;
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
			{"RandoCyclopsDoll", TechType.Cyclops},
			{"RandoRocketBaseDoll", TechType.RocketBase}
		};
		public static HashSet<TechType> Resizables = new() { TechType.Seamoth, TechType.Exosuit, TechType.Cyclops, TechType.RocketBase };

		[HarmonyPatch(typeof(Vehicle))]
		[HarmonyPatch(nameof(Vehicle.OnHandHover))]
		[HarmonyPrefix]
		public static bool PatchExosuitDoll(Vehicle __instance)
		{
			PluginSetup.BepinExLogger.LogInfo($"Check out {__instance.GetType()}");
			if (__instance.GetType() == typeof(Exosuit))
			{
				return true;
			}
			return true;
		}
		[HarmonyPatch(typeof(Pickupable))]
		[HarmonyPatch(nameof(Pickupable.Drop), new[] { typeof(Vector3), typeof(Vector3), typeof(bool) })]
		[HarmonyPrefix]
		public static void PatchVehicleDropPre(Pickupable __instance)
		{
			if (__instance.GetTechType() == TechType.Cyclops)
			{
				__instance.randomizeRotationWhenDropped = false;
			}
		}
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
	}
}