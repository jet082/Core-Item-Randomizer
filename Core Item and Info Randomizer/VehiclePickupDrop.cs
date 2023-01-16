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
                    var exoSuitVar = __instance.GetComponentInParent<Vehicle>();
					var cheatKeyCode = exoSuitVar.gameObject.EnsureComponent<KeypadDoorConsole>();
					cheatKeyCode.accessCode = "9876";
					exoSuitVar.timeOnGround = 0f;
                    exoSuitVar.onGround = false;
                }
            }
        }
		[HarmonyPatch(typeof(Vehicle))]
		[HarmonyPatch(nameof(Vehicle.Update))]
		[HarmonyPostfix]
		public static void FixVehicleRotation(Vehicle __instance)
		{
			if (__instance is Exosuit)
			{
				var cheatKeyCode = __instance.gameObject.EnsureComponent<KeypadDoorConsole>();
				if (cheatKeyCode.accessCode == "9876")
				{
					__instance.transform.localRotation = new Quaternion(0f, 0f, 0f, 1f);
					cheatKeyCode.accessCode = "0000";
				}
			}
		}
	}
}
