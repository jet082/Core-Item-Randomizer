using SMLHelper.V2.Json.Attributes;
using SMLHelper.V2.Json;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	[FileName("RandomizerSaveData")]
	public class SaveData : SaveDataCache
	{
		public Dictionary<string, string> PDAData = new();
		public Dictionary<Vector3, string> ChestPlacementData = new();
		public Dictionary<string, string> Codes = new();
	}
}