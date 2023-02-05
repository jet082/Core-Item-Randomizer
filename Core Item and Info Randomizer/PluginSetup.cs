using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SMLHelper.V2.Handlers;

namespace CoreItemAndInfoRandomizer
{
	[BepInPlugin(myGUID, pluginName, versionString)]
	public class PluginSetup : BaseUnityPlugin
	{
		private const string myGUID = "com.jet082.coreitemandinforandomizer";
		private const string pluginName = "Core Item and Info Randomizer";
		private const string versionString = "0.0.1";
		private static readonly Harmony harmony = new(myGUID);
		public static ManualLogSource BepinExLogger;
		public static bool DebugMode = false;
		public static SaveData RandomizerLoadedSaveData = SaveDataHandler.Main.RegisterSaveDataCache<SaveData>();
		public static int Seed = 2139831802;
		public static bool CompletelyRandomFishSizes = false;
		private void Awake()
		{
			base.Logger.LogInfo(pluginName + " " + versionString + " " + "loaded.");
			BepinExLogger = base.Logger;
			harmony.PatchAll();
		}
	}
}