using BepInEx;
using SMLHelper.V2.Handlers;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class BoxPlacement
	{
		public static void PlaceChests()
		{
			foreach (KeyValuePair<Vector3, object> someBox in BoxPlacementsData.BoxPlacementDict())
			{
				SpawnInfo chestSpawn = new("580154dd-b2a3-4da1-be14-9a22e20385c8", someBox.Key);
				CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(chestSpawn);
			}
		}
	}
}