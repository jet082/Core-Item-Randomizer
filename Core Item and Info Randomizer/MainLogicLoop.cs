using CoreItemAndInfoRandomizer.Creatures;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using SMLHelper.V2.Handlers;
using System.Collections.Generic;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch(typeof(Player))]
	public class MainLogicLoop
	{
		public static Dictionary<string, JObject> GameLogic = SaveAndLoad.LoadLogic("DefaultLogic.json");
		[HarmonyPatch(nameof(Player.Start))]
		[HarmonyPostfix]
		public static void RunMainLogic()
		{
			UnityEngine.Random.InitState(PluginSetup.Seed);
			PDAPatcher.GeneratePDAEntries();
			RandomizeFishSpecies.Randomize();
			new MyVeryOwnSupplyCrate().Patch();
			new RandoSeamothDoll().Patch();
			new RandoRocketBaseDoll().Patch();
			new RandoCyclopsDoll().Patch();
			new RandoPrawnSuitDoll().Patch();
			new RandoRocketBaseLadderDoll().Patch();
			new RandoRocketStage1Doll().Patch();
			new RandoRocketStage2Doll().Patch();
			new RandoRocketStage3Doll().Patch();
			new AmyThePeeperLeviathan().Patch();
			ModCache.Setup();
			CratePlacementsData.Setup();
			Placement.PlaceChests();
			Placement.PlaceLeviathans();
		}
	}
}