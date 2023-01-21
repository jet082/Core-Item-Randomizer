using Newtonsoft.Json.Linq;
using SMLHelper.V2.Handlers;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class Placement
	{
		public static void PlaceChests()
		{
			foreach (Vector3 someKey in CratePlacementsData.BoxPlacementDictionary().Keys)
			{
				SpawnInfo chestSpawn = new(ModCache.CacheData["MyVeryOwnSupplyCrate"].ClassId, someKey);
				CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(chestSpawn);
			}
		}
		public static void PlaceLeviathans()
		{
			SpawnInfo amySpawn = new(ModCache.CacheData["AmyThePeeperLeviathan"].ClassId, new Vector3(-575f, -48f, -62f));
			CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(amySpawn);
		}
	}
}