using System.Collections.Generic;

namespace CoreItemAndInfoRandomizer.Creatures
{
	public class RandomizeFishSpecies
	{
		public static HashSet<string> ArrayOfFishes = new()
		{
			TechType.Shocker.ToString(),
			TechType.Biter.ToString(),
			TechType.Blighter.ToString(),
			TechType.BoneShark.ToString(),
			TechType.Crabsnake.ToString(),
			TechType.CrabSquid.ToString(),
			TechType.Crash.ToString(),
			TechType.LavaLarva.ToString(),
			TechType.Mesmer.ToString(),
			TechType.SpineEel.ToString(),
			TechType.Sandshark.ToString(),
			TechType.Stalker.ToString(),
			TechType.Warper.ToString(),
			TechType.Bladderfish.ToString(),
			TechType.Boomerang.ToString(),
			TechType.GhostRayRed.ToString(),
			TechType.Cutefish.ToString(),
			TechType.Eyeye.ToString(),
			TechType.GarryFish.ToString(),
			TechType.Gasopod.ToString(),
			TechType.GhostRayBlue.ToString(),
			TechType.HoleFish.ToString(),
			TechType.Hoopfish.ToString(),
			TechType.Hoverfish.ToString(),
			TechType.Jellyray.ToString(),
			TechType.LavaBoomerang.ToString(),
			TechType.Oculus.ToString(),
			TechType.Peeper.ToString(),
			TechType.RabbitRay.ToString(),
			TechType.LavaEyeye.ToString(),
			TechType.Reginald.ToString(),
			TechType.Skyray.ToString(),
			TechType.Spadefish.ToString(),
			TechType.Spinefish.ToString(),
			TechType.BlueAmoeba.ToString(),
			TechType.LargeFloater.ToString(),
			TechType.Bleeder.ToString(),
			TechType.Shuttlebug.ToString(),
			TechType.CaveCrawler.ToString(),
			TechType.Floater.ToString(),
			TechType.LavaLarva.ToString(),
			TechType.Rockgrub.ToString(),
			TechType.Shuttlebug.ToString(),
			TechType.BrainCoral.ToString(),
			TechType.BrownTubes.ToString(),
			TechType.BigCoralTubes.ToString(),
			TechType.RedTipRockThings.ToString(),
			TechType.BlueJeweledDisk.ToString(),
			TechType.GreenJeweledDisk.ToString(),
			TechType.PurpleJeweledDisk.ToString(),
			TechType.RedJeweledDisk.ToString(),
			TechType.TreeMushroom.ToString(),
			TechType.CoralShellPlate.ToString(),
			TechType.GhostLeviathan.ToString(),
			TechType.GhostLeviathanJuvenile.ToString(),
			TechType.ReaperLeviathan.ToString(),
			TechType.Reefback.ToString(),
			TechType.ReefbackBaby.ToString(),
			TechType.SeaDragon.ToString(),
			TechType.SeaEmperor.ToString(),
			TechType.SeaEmperorBaby.ToString(),
			TechType.SeaEmperorJuvenile.ToString(),
			TechType.SeaTreader.ToString()
		};
		public static void Randomize()
		{
			SaveData fishSaveData = PluginSetup.RandomizerLoadedSaveData;
			if (fishSaveData.FishSpeciesScaling.Count == 0)
			{
				foreach (string someFish in ArrayOfFishes)
				{
					float scaleFactor = UnityEngine.Random.Range(1f, 3f);
					float finalScale;
					if (UnityEngine.Random.value > .5)
					{
						finalScale = (1 / scaleFactor);
					}
					else
					{
						finalScale = scaleFactor;
					}
					fishSaveData.FishSpeciesScaling[someFish] = finalScale.ToString();
				}
			}
		}
	}
}
