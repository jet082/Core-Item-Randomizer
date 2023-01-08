using HarmonyLib;
using UnityEngine;
using BepInEx.Logging;
using System.Collections.Generic;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch(typeof(HandTarget))]
	public class BoxPatcher
	{
		[HarmonyPatch(nameof(HandTarget.Awake))]
		[HarmonyPostfix]
		public static void Awake_Prefix(HandTarget __instance)
		{
			if (__instance.GetType() == typeof(SupplyCrate))
			{
				SupplyCrate someSupplyCrate = __instance as SupplyCrate;
				PluginSetup.logger.Log(LogLevel.Info, $"Coordinate of Crate is {someSupplyCrate.transform.position}");
				if (someSupplyCrate.transform.position.Equals(new Vector3(0f, 0f, 0f)))
				{
					PrefabPlaceholdersGroup pre = someSupplyCrate.gameObject.EnsureComponent<PrefabPlaceholdersGroup>();
					someSupplyCrate.gameObject.EnsureComponent<Sealed>()._sealed = true;
					pre.prefabPlaceholders[0].prefabClassId = CraftData.GetClassIdForTechType(TechType.Seamoth);
					pre.prefabPlaceholders[0].gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
					pre.prefabPlaceholders[0].gameObject.EnsureComponent<Pickupable>();
				}
			}
		}
	}
}