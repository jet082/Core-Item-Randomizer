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
			{"RandoCyclopsDoll", TechType.Cyclops},
			{"RandoRocketBaseDoll", TechType.RocketBase}
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
	}
}