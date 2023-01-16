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
			new RandoSeamothDoll().Patch();
		}
	}
}