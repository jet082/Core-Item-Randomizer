using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CoreItemAndInfoRandomizer.Logic
{
	public class LogicPlacementData
	{
		public static HashSet<string> WorkInProgress;
		public static string[] Locations =
		{

		};
		public static void SetupPlayerStart()
		{

		}
		public static Vector3[] FindAllLogicalChecks()
		{
			return new Vector3[1];
		}
		public static void PlaceRequiredChestItems()
		{

		}
		public static void PlaceNonRequiredChestItems()
		{
			foreach (Vector3 someKey in CratePlacementsData.BoxPlacements.Keys)
			{
				if (!PluginSetup.CachedRandoData.ChestPlacements.ContainsKey(someKey.ToString()))
				{
					int randomKeyIndex = UnityEngine.Random.Range(0, CratePlacementsData.DistributionTable.Count);
					string toAddPre1 = CratePlacementsData.DistributionTable.ElementAt(randomKeyIndex).Key;
					string toAddPre2 = CratePlacementsData.DistributionTable.ElementAt(randomKeyIndex).Value.HumanReadable;
					string[] toAdd = new string[] { toAddPre1, toAddPre2 };
					PluginSetup.CachedRandoData.ChestPlacements.Add(someKey.ToString(), toAdd);
					string humanReadableChestName = (string)LogicParser.GameLogic["supplyCrateCoordinates"][someKey.ToString()]["chestName"];
					PluginSetup.SpoilerLogData.ChestPlacements.Add(humanReadableChestName, toAddPre2);
				}
			}
		}
		public static void InitializeData()
		{
			SetupPlayerStart();
			PlaceRequiredChestItems();
			PlaceNonRequiredChestItems();
		}
	}
}
