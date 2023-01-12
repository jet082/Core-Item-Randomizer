using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class ItemResizer
	{
		[HarmonyPatch(typeof(Pickupable))]
		[HarmonyPatch(nameof(Pickupable.Pickup))]
		[HarmonyPostfix]
		public static void PatchVehicle(Pickupable __instance)
		{
			Pickupable someVehicle = __instance;
			if (someVehicle.GetTechType() == TechType.Seamoth) {
				someVehicle.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
	}
}
