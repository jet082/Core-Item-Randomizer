﻿using SMLHelper.V2.Json.Attributes;
using SMLHelper.V2.Json;
using System.Collections.Generic;

namespace CoreItemAndInfoRandomizer
{
	[FileName("RandomizerSaveData")]
	public class SaveData : SaveDataCache
	{
		public Dictionary<string, string> PDAData = new();
		public Dictionary<string, string> ChestPlacementData = new();
		public Dictionary<string, string> Codes = new();
		public Dictionary<string, string> FishSpeciesScaling = new();
	}
}