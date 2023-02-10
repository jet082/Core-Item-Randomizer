using SMLHelper.V2.Json;
using SMLHelper.V2.Json.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemRandomizer
{
	[FileName("Randomizer Cache Data")]
	public class RandomizerCacheData : SaveDataCache
	{
		public Tuple<bool, int> SeedData = new(false, 0);
		public Dictionary<string, string[]> ChestPlacements = new();
		public Dictionary<string, string> PDAData = new();
		public Dictionary<string, string> Codes = new();
		public Dictionary<string, float> Scaling = new();
		public HashSet<string> ObtainedItems = new();
		public Vector3 StartingLocation = Vector3.zero;
	}
}