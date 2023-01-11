using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class BoxPlacementsData
	{
		public static List<Tuple<Vector3, JToken>> BoxPlacementTuples()
		{
			IDictionary<string, JToken> tempDictData = (IDictionary<string, JToken>)MainLogicLoop.GameLogic["supplyCrateCoordinates"];
			List<Tuple<Vector3, JToken>> finalTuples = new();
			foreach (KeyValuePair<string, JToken> someData in tempDictData)
			{
				string[] vectorData = someData.Key.Split(',');
				Vector3 vectorized = new(float.Parse(vectorData[0]), float.Parse(vectorData[1]), float.Parse(vectorData[2]));
				finalTuples.Add(Tuple.Create(vectorized, someData.Value));
			}
			return finalTuples;
		}
	}
}