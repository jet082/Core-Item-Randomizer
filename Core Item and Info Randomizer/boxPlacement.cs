using BepInEx;
using SMLHelper.V2.Handlers;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class BoxPlacement
	{
		public static void placeChests()
		{
			foreach (KeyValuePair<Vector3, List<string>> someBox in BoxPlacementsData.BoxPlacementsAndRequirements)
			{
				SpawnInfo chestSpawn = new SpawnInfo("580154dd-b2a3-4da1-be14-9a22e20385c8", someBox.Key);
				CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(chestSpawn);
			}
		}
	}
}