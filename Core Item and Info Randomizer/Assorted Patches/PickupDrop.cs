using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class PickupDrop
	{
		public static Dictionary<string, TechType> DollToVehicleTranslation = new()
		{
			{"RandoSeamothDoll", TechType.Seamoth},
			{"RandoPrawnSuitDoll", TechType.Exosuit},
			{"RandoCyclopsDoll", TechType.Cyclops},
			{"RandoRocketBaseDoll", TechType.RocketBase}
		};
		public static HashSet<TechType> Resizables = new() { TechType.Seamoth, TechType.Exosuit, TechType.Cyclops, TechType.RocketBase };

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
			if (__instance.open)
			{
				return StopClick(__instance.itemInside.gameObject, __instance.itemInside.GetTechName(), __instance.itemInside.GetTechType());
			}
			return true;
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
			someObject.transform.localScale = Vector3.one;
			if (DollToVehicleTranslation.ContainsKey(someString))
			{
				GameObject.Destroy(someObject);
				CraftData.AddToInventory(DollToVehicleTranslation[someString]);
				return false;
			}
			return true;
		}
	}
}