using SMLHelper.V2.Json.Attributes;
using SMLHelper.V2.Json;
using System.Collections.Generic;

namespace CoreItemAndInfoRandomizer
{
	[FileName("PDAData")]
	public class SaveData : SaveDataCache
	{
		public Dictionary<string, string> PDAData = new();
	}
}