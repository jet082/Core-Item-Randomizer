using CoreItemAndInfoRandomizer.Creatures;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class RandomFishSize
	{
		public static HashSet<string> ToScaleScale = new() {
			TechType.SnakeMushroom.ToString(),
			TechType.TreeMushroom.ToString()
		};
		[HarmonyPatch(typeof(Creature))]
		[HarmonyPatch(nameof(Creature.Start))]
		[HarmonyPrefix]
		public static void RandomizeTheFish(Creature __instance)
		{
			if (!__instance.GetComponent<SpecialResizable>())
			{
				if (PluginSetup.CompletelyRandomFishSizes)
				{
					float scaleFactor = UnityEngine.Random.Range(1f, 3f);
					if (UnityEngine.Random.value > .5)
					{
						__instance.SetScale(1 / scaleFactor);
					}
					else
					{
						__instance.SetScale(scaleFactor);
					}
				}
				else
				{
					string creatureTechType = CraftData.GetTechType(__instance.gameObject).ToString();
					if (RandomizeFishSpecies.ArrayOfFishes.Contains(creatureTechType))
					{
						__instance.SetScale(PluginSetup.RandomizerLoadedSaveData.FishSpeciesScaling[creatureTechType]);
					}
				}
			}
		}
		[HarmonyPatch(typeof(LargeWorldEntity))]
		[HarmonyPatch(nameof(LargeWorldEntity.Start))]
		[HarmonyPrefix]
		public static void ResizeLWE(LargeWorldEntity __instance)
		{
			string creatureTechType = CraftData.GetTechType(__instance.gameObject).ToString();
			if (!__instance.GetComponent<SpecialResizable>())
			{
				ResizeStuff(__instance.gameObject, creatureTechType);
			}
		}
		/*		[HarmonyPatch(typeof(BreakableResource))]
				[HarmonyPatch(nameof(BreakableResource.Awake))]
				[HarmonyPrefix]
				public static void ResizeBreakable(BreakableResource __instance)
				{
					string creatureTechType;
					if (__instance.defaultPrefabReference.ToString() == "484975a7c9dc5644b934c51e42cef239")
					{
						creatureTechType = __instance.defaultPrefabReference.ToString();
					}
					else
					{
						creatureTechType = CraftData.GetTechType(__instance.gameObject).ToString();
					}
					if (!__instance.GetComponent<SpecialResizable>())
					{
						ResizeStuff(__instance.gameObject, creatureTechType);
					}
				}*/
		public static void ResizeStuff(GameObject thingToResize, string creatureTechType)
		{			
			if (RandomizeFishSpecies.ArrayOfFishes.Contains(creatureTechType) || creatureTechType == "484975a7c9dc5644b934c51e42cef239")
			{
				PluginSetup.BepinExLogger.LogInfo($"Resizing {creatureTechType}");
				Vector3 toAdjust;
				if (ToScaleScale.Contains(creatureTechType))
				{
					toAdjust = thingToResize.transform.localScale;
				}
				else
				{
					toAdjust = Vector3.one;
				}
				if (PluginSetup.CompletelyRandomFishSizes)
				{
					float scaleFactor = UnityEngine.Random.Range(1f, 3f);
					if (UnityEngine.Random.value > .5)
					{
						thingToResize.transform.localScale = new Vector3(toAdjust.x * 1 / scaleFactor, toAdjust.y * 1 / scaleFactor, toAdjust.z * 1 / scaleFactor);
					}
					else
					{
						thingToResize.transform.localScale = new Vector3(toAdjust.x * scaleFactor, toAdjust.y * scaleFactor, toAdjust.z * scaleFactor);
					}
				}
				else
				{
					float todo = PluginSetup.RandomizerLoadedSaveData.FishSpeciesScaling[creatureTechType];
					thingToResize.transform.localScale = new Vector3(toAdjust.x * todo, toAdjust.y * todo, toAdjust.z * todo);
				}
			}
		}
	}
}