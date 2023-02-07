using SMLHelper.V2.Handlers;
using System.Linq;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class Placement
	{
		public static void PlaceChests()
		{
			SaveData saveData = PluginSetup.RandomizerLoadedSaveData;
			var placementData = CratePlacementsData.BoxPlacements;
			if (PluginSetup.DebugCrates)
			{
				float offsetValue = 0;
				foreach (string someContents in CratePlacementsData.DistributionTable.Keys)
				{
					Vector3 toDoVectorPlacement = new(-712.6f + offsetValue, -3f, -733.56f);
					SpawnInfo chestSpawn = new(ModCache.CacheData["MyVeryOwnSupplyCrate"].ClassId, toDoVectorPlacement);
					CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(chestSpawn);
					offsetValue += 3;
				}
			}
			foreach (Vector3 someKey in placementData.Keys)
			{
				SpawnInfo chestSpawn = new(ModCache.CacheData["MyVeryOwnSupplyCrate"].ClassId, someKey);
				CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(chestSpawn);
				if (!saveData.ChestPlacementData.ContainsKey(someKey.ToString()))
				{
					int randomKeyIndex = UnityEngine.Random.Range(0, CratePlacementsData.DistributionTable.Count);
					saveData.ChestPlacementData.Add(someKey.ToString(), new string[] { CratePlacementsData.DistributionTable.ElementAt(randomKeyIndex).Key, CratePlacementsData.DistributionTable.ElementAt(randomKeyIndex).Value.HumanReadable });
				}
			}
		}
		public static void PlaceLeviathans()
		{
			SpawnInfo amySpawn = new(ModCache.CacheData["AmyThePeeperLeviathan"].ClassId, new Vector3(-575f, -48f, -62f));
			CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(amySpawn);
		}
		public static void PlaceEverything()
		{
			PlaceChests();
			PlaceLeviathans();
		}
	}
}