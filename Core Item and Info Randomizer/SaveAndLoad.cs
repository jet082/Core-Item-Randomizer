using SMLHelper.V2.Utility;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace CoreItemAndInfoRandomizer
{
	public class SaveAndLoad
	{
		public static string SavePath = SaveUtils.GetCurrentSaveDataDir();
		public static Dictionary<string, object> LoadLogic(string logicFileName)
		{
			string json = File.ReadAllText(logicFileName);
			return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
		}
	}
}
