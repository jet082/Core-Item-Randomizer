using System.Collections.Generic;

namespace CoreItemAndInfoRandomizer
{
	public class MainLogicLoop
	{
		public static Dictionary<string, object> GameLogic = SaveAndLoad.LoadLogic("DefaultLogic.json");
		public static void RunMainLogic()
		{
			PDAPatcher.GeneratePDAEntries();
			BoxPlacement.PlaceChests();
		}
	}
}