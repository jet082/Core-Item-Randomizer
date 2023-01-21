using HarmonyLib;
using SMLHelper.V2.Handlers;
using System.Collections.Generic;

namespace CoreItemAndInfoRandomizer
{
	[HarmonyPatch(typeof(Player))]
	public class MainLogicLoop
	{
		public static Dictionary<string, object> GameLogic = SaveAndLoad.LoadLogic("DefaultLogic.json");
		[HarmonyPatch(nameof(Player.Start))]
		[HarmonyPostfix]
		public static void RunMainLogic()
		{
			PDAPatcher.GeneratePDAEntries();
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
			Placement.PlaceChests();
			Placement.PlaceLeviathans();
			CratePlacementsData.Setup();
		}
	}
}