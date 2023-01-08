using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace Core_Item_and_Info_Randomizer
{
	[BepInPlugin(myGUID, pluginName, versionString)]
	public partial class PluginSetup : BaseUnityPlugin
	{
		private const string myGUID = "com.jet082.coreitemandinforandomizer";
		private const string pluginName = "Core Item and Info Randomizer";
		private const string versionString = "1.0.0";
		private static readonly Harmony harmony = new Harmony(myGUID);
		public static ManualLogSource logger;
		private void Awake()
		{
			pdaPatcher.generatePdaEntries();
			boxPlacement.placeChests();
			harmony.PatchAll();
			Logger.LogInfo(pluginName + " " + versionString + " " + "loaded.");
			logger = Logger;
		}
	}
}