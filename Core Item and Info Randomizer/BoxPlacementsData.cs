using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class BoxPlacementsData
	{
		public static Dictionary<Vector3, object> BoxPlacementDict()
		{
			Dictionary<string, object> tempDictData = (Dictionary<string, object>)MainLogicLoop.GameLogic["supplyCrateCoordinates"];
			Dictionary<Vector3, object> finalDict = new();
			foreach (KeyValuePair<string, object> someData in tempDictData)
			{
				string[] vectorData = someData.Key.Split(',');
				Vector3 vectorized = new(float.Parse(vectorData[0]), float.Parse(vectorData[1]), float.Parse(vectorData[2]));
				finalDict[vectorized] = someData.Value;
			}
			return finalDict;
		}
	}
}