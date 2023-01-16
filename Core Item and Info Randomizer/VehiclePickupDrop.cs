using HarmonyLib;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class VehiclePickupDrop
	{
		[HarmonyPatch(typeof(Pickupable))]
		[HarmonyPatch(nameof(Pickupable.Pickup))]
		[HarmonyPostfix]
		public static void PatchVehiclePickup(Pickupable __instance)
		{
			if (__instance.GetTechType() == TechType.Seamoth || __instance.GetTechType() == TechType.Exosuit) {
				__instance.transform.localScale = new Vector3(1f, 1f, 1f);
			} else if (__instance.GetTechType() == TechType.StarshipSouvenir)
			{
			}
		}
		[HarmonyPatch(typeof(Pickupable))]
		[HarmonyPatch(nameof(Pickupable.Drop), new[] { typeof(Vector3), typeof(Vector3), typeof(bool) })]
		[HarmonyPrefix]
		public static void PatchVehicleDropPre(Pickupable __instance)
		{
			if (__instance.GetTechType() == TechType.Exosuit)
			{
				__instance.randomizeRotationWhenDropped = false;
			}
		}
		[HarmonyPatch(typeof(Pickupable))]
        [HarmonyPatch(nameof(Pickupable.Drop), new[] { typeof(Vector3), typeof(Vector3), typeof(bool) })]
        [HarmonyPostfix]
        public static void PatchVehicleDrop(Pickupable __instance)
        {
            if (__instance.GetTechType() == TechType.Seamoth || __instance.GetTechType() == TechType.Exosuit)
            {
                if (__instance.GetTechType() == TechType.Exosuit)
                {
                    Vehicle exoSuitVar = __instance.GetComponentInParent<Vehicle>();
					exoSuitVar.timeOnGround = 0f;
                    exoSuitVar.onGround = false;
                }
            }
        }
	}
}