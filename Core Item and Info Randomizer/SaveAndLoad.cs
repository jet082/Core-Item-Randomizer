using SMLHelper.V2.Utility;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Json;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class SaveAndLoad
	{
		public static string SavePath = SaveUtils.GetCurrentSaveDataDir();
		public static string PluginPath = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory?.FullName;
		public static readonly AssetBundle Assets = AssetBundle.LoadFromFile(Path.Combine(PluginPath, "coreitemrandomizer.assets"));
		public static Dictionary<string, object> LoadLogic(string logicFileName)
		{
			string json = File.ReadAllText(Path.Combine(PluginPath, logicFileName));
			return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
		}
	}
}