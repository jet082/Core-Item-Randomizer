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
		private const string versionString = "1.0.0";
		private static readonly Harmony harmony = new(myGUID);
		public static ManualLogSource BepinExLogger;
		public static SaveData RandomizerLoadedSaveData = SaveDataHandler.Main.RegisterSaveDataCache<SaveData>();
		private void Awake()
		{
			base.Logger.LogInfo(pluginName + " " + versionString + " " + "loaded.");
			BepinExLogger = base.Logger;
			BoxPlacement.PlaceChests();
			harmony.PatchAll();
		}
	}
}