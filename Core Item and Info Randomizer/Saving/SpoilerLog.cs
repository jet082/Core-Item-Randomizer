using SMLHelper.V2.Json;
using SMLHelper.V2.Json.Attributes;
using System.Collections.Generic;

namespace CoreItemAndInfoRandomizer
{
	[FileName("Spoiler Log")]
	public class SpoilerLog : SaveDataCache
	{
		public Dictionary<string, string> ChestPlacements = new();
		public Dictionary<string, string> PDAData = new();
		public Dictionary<string, string> Codes = new();
		public Dictionary<string, float> Scaling = new();
		public string StartingLocation = "";
	}
}