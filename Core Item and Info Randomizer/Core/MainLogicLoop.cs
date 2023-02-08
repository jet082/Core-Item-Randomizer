using CoreItemAndInfoRandomizer.Core;
using CoreItemAndInfoRandomizer.ModItems;
using CoreItemAndInfoRandomizer.Saving;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch(typeof(Player))]
	public class MainLogicLoop
	{
		public static Dictionary<string, JObject> GameLogic = SaveAndLoad.LoadLogic("DefaultLogic.json");
		public static void DebugWrite(string someString)
		{
			if (PluginSetup.DebugMode)
			{
				PluginSetup.BepinExLogger.LogInfo(someString);
			}
		}
		[HarmonyPatch(nameof(Player.Start))]
		[HarmonyPostfix]
		public static void RunMainLogic()
		{
			SetupSeedRandomization.InitializeSeed();
			InitializeCustomElements.Initialize();
			Randomize.Initialize();
			CratePlacementsData.Setup();
			Placement.PlaceEverything();
		}
	}
}