using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SMLHelper.V2.Utility;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CoreItemRandomizer
{
	public class SaveAndLoad
	{
		public static string SavePath = SaveUtils.GetCurrentSaveDataDir();
		public static string PluginPath = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory?.FullName;
		public static Dictionary<string, JObject> LoadLogic(string logicFileName)
		{
			string json = File.ReadAllText(Path.Combine(PluginPath, logicFileName));
			return JsonConvert.DeserializeObject<Dictionary<string, JObject>>(json);
		}
	}
}