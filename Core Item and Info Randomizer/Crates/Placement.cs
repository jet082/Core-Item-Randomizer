using SMLHelper.V2.Handlers;
using System.Linq;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class Placement
	{
		public static void PlaceChests()
		{
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
				if (!PluginSetup.CachedRandoData.ChestPlacements.ContainsKey(someKey.ToString()))
				{
					int randomKeyIndex = UnityEngine.Random.Range(0, CratePlacementsData.DistributionTable.Count);
					string toAddPre1 = CratePlacementsData.DistributionTable.ElementAt(randomKeyIndex).Key;
					string toAddPre2 = CratePlacementsData.DistributionTable.ElementAt(randomKeyIndex).Value.HumanReadable;
					string[] toAdd = new string[] { toAddPre1, toAddPre2 };
					PluginSetup.CachedRandoData.ChestPlacements.Add(someKey.ToString(), toAdd);
					PluginSetup.SpoilerLogData.ChestPlacements.Add(someKey.ToString(), toAddPre2);
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