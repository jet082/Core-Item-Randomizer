using Newtonsoft.Json.Linq;
using SMLHelper.V2.Handlers;
using System;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class BoxPlacement
	{
		public static void PlaceChests()
		{
			foreach (Tuple<Vector3, JToken> someTuple in BoxPlacementsData.BoxPlacementTuples())
			{
				SpawnInfo chestSpawn = new("580154dd-b2a3-4da1-be14-9a22e20385c8", someTuple.Item1);
				CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(chestSpawn);
			}
		}
	}
}