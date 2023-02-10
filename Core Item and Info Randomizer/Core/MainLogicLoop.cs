using CoreItemRandomizer.Core;
using CoreItemRandomizer.Logic;
using CoreItemRandomizer.ModItems;
using CoreItemRandomizer.Saving;
using HarmonyLib;

namespace CoreItemRandomizer
{
	[HarmonyPatch(typeof(Player))]
	public class MainLogicLoop
	{
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
			LogicPlacementData.InitializeData();
		}
	}
}