using HarmonyLib;
using UnityEngine;
using BepInEx.Logging;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch(typeof(HandTarget))]
	public class BoxPatcher
	{
		[HarmonyPatch(nameof(HandTarget.Awake))]
		[HarmonyPostfix]
		public static void PatchHandTarget(HandTarget __instance)
		{
			if (__instance.GetType() == typeof(SupplyCrate))
			{
				SupplyCrate someSupplyCrate = __instance as SupplyCrate;
				PluginSetup.BepinExLogger.Log(LogLevel.Info, $"Coordinate of Crate is {someSupplyCrate.transform.position}");
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