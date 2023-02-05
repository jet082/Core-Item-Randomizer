using Newtonsoft.Json.Linq;
using SMLHelper.V2.Handlers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class Placement
	{
		public static Dictionary<Vector3, string> ChestContents = new();
		public static void PlaceChests()
		{
			var placementData = CratePlacementsData.BoxPlacements;
			if (PluginSetup.DebugMode)
			{
				float offsetValue = 0;
				foreach (string someContents in CratePlacementsData.DistributionTable.Keys)
				{
					Vector3 toDoVectorPlacement = new(-712.6f + offsetValue, -3f, -733.56f);
					ChestContents[toDoVectorPlacement] = someContents;
					SpawnInfo chestSpawn = new(ModCache.CacheData["MyVeryOwnSupplyCrate"].ClassId, toDoVectorPlacement);
					CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(chestSpawn);
					offsetValue += 3;
				}
			}
			else
			{
				foreach (Vector3 someKey in placementData.Keys)
				{
					SpawnInfo chestSpawn = new(ModCache.CacheData["MyVeryOwnSupplyCrate"].ClassId, someKey);
					CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(chestSpawn);
					UnityEngine.Random.InitState(DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond);
					int randomKeyIndex = UnityEngine.Random.Range(0, CratePlacementsData.DistributionTable.Count);
					ChestContents[someKey] = CratePlacementsData.DistributionTable.ElementAt(randomKeyIndex).Key;
				}
			}
		}
		public static void PlaceLeviathans()
		{
			SpawnInfo amySpawn = new(ModCache.CacheData["AmyThePeeperLeviathan"].ClassId, new Vector3(-575f, -48f, -62f));
			CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(amySpawn);
		}
	}
}