using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CoreItemAndInfoRandomizer
{
	[BepInPlugin(myGUID, pluginName, versionString)]
	public class PluginSetup : BaseUnityPlugin
	{
		private const string myGUID = "com.jet082.coreitemandinforandomizer";
		private const string pluginName = "Core Item and Info Randomizer";
		private const string versionString = "1.0.0";
		private static readonly Harmony harmony = new(myGUID);
		public static ManualLogSource logger;
		private void Awake()
		{
			PDAPatcher.GeneratePDAEntries();
			BoxPlacement.PlaceChests();
			harmony.PatchAll();
			Logger.LogInfo(pluginName + " " + versionString + " " + "loaded.");
			logger = Logger;
		}
	}
}