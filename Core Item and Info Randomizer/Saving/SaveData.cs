using SMLHelper.V2.Json;
using SMLHelper.V2.Json.Attributes;
using System.Collections.Generic;

namespace CoreItemAndInfoRandomizer
{
	[FileName("RandomizerSaveData")]
	public class SaveData : SaveDataCache
	{
		public Dictionary<string, string[]> ChestPlacementData = new();
		public Dictionary<string, string> PDAData = new();
		public Dictionary<string, string> Codes = new();
		public Dictionary<string, float> FishSpeciesScaling = new();
		public HashSet<string> ObtainedItems = new();
	}
}