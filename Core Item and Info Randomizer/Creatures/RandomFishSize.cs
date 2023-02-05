using CoreItemAndInfoRandomizer.Creatures;
using HarmonyLib;
using System;
using System.Linq;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch]
	public class RandomFishSize
	{
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
						__instance.SetScale(float.Parse(PluginSetup.RandomizerLoadedSaveData.FishSpeciesScaling[creatureTechType]));
					}
				}
			}
		}
	}
}