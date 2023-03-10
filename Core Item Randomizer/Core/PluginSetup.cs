using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SMLHelper.V2.Handlers;

namespace CoreItemRandomizer
{
	[BepInPlugin(myGUID, pluginName, versionString)]
	public class PluginSetup : BaseUnityPlugin
	{
		private const string myGUID = "com.jet082.coreItemRandomizer";
		private const string pluginName = "Core Item Randomizer";
		private const string versionString = "0.0.1";
		private static readonly Harmony harmony = new(myGUID);

		public static bool DebugMode = true;
		public static bool DebugCrates = false;
		public static int Seed = 2139831802;
		public static bool CompletelyRandomFishSizes = false;
		public static ManualLogSource BepinExLogger;
		public static SpoilerLog SpoilerLogData = SaveDataHandler.Main.RegisterSaveDataCache<SpoilerLog>();
		public static RandomizerCacheData CachedRandoData = SaveDataHandler.Main.RegisterSaveDataCache<RandomizerCacheData>();
		public static bool RandomStartingLocation = true;
		public static int Iterations = 90000;
		private void Awake()
		{
			base.Logger.LogInfo(pluginName + " " + versionString + " " + "loaded.");
			BepinExLogger = base.Logger;
			harmony.PatchAll();
		}
	}
}