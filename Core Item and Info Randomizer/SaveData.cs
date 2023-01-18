using SMLHelper.V2.Json.Attributes;
using SMLHelper.V2.Json;
using System.Collections.Generic;

namespace CoreItemAndInfoRandomizer
{
	[FileName("RandomizerSaveData")]
	public class SaveData : SaveDataCache
	{
		public Dictionary<string, string> PDAData = new();
		public bool HasLoadedCyclopsYet;
	}
}