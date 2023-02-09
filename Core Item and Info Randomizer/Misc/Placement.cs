using SMLHelper.V2.Handlers;
using System.Linq;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class Placement
	{
		public static void DetermineCheckAccess()
		{

		}
		public static void Place(string classId, Vector3 placementVector)
		{
			SpawnInfo chestSpawn = new(classId, placementVector);
			CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(chestSpawn);
		}
		public static void PlaceChests()
		{
			var placementData = CratePlacementsData.BoxPlacements;
			if (PluginSetup.DebugCrates)
			{
				float offsetValue = 0;
				foreach (string someContents in CratePlacementsData.DistributionTable.Keys)
				{
					Vector3 toDoVectorPlacement = new(-712.6f + offsetValue, -3f, -733.56f);
					Place(ModCache.CacheData["MyVeryOwnSupplyCrate"].ClassId, toDoVectorPlacement);
					offsetValue += 3;
				}
			}
			foreach (Vector3 someKey in placementData.Keys)
			{
				Place(ModCache.CacheData["MyVeryOwnSupplyCrate"].ClassId, someKey);
			}
		}
		public static void PlaceLeviathans()
		{
			Place(ModCache.CacheData["AmyThePeeperLeviathan"].ClassId, new Vector3(-575f, -48f, -62f));
		}
		public static void PlaceEverything()
		{
			PlaceChests();
			PlaceLeviathans();
		}
	}
}