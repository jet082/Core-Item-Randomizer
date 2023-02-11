using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CoreItemRandomizer.Logic
{
	public class LogicPlacementData
	{
		public static Dictionary<string, Tuple<string, int>> ItemsLeftToDistribute = new();
		public static HashSet<Vector3> LocationsLeftToDistribute = new();
		public static HashSet<Vector3> RequiredLocations = new();
		public static Dictionary<Vector3, string> LocationTranslation = new() {
			{ new Vector3(-110.6f, 0f, -347.1f), "SafeShallowsEStart" },
			{ new Vector3(-728.0f, -761f, -274.7f), "LostRiverSStart" },
			{ new Vector3(401.2f, 0f, -524.1f), "CrashZoneStart" },
			{ new Vector3(1001.9f, 0, -1120.3f), "CrashZoneStart" }
		};
		public static HashSet<string> GetAllAccessibleTokens(HashSet<string> obtainedItems)
		{
			HashSet<string> accessible = new();
			HashSet<string> nextSphere = obtainedItems;
			while (nextSphere.Count > 0)
			{
				accessible.UnionWith(nextSphere);
				nextSphere = new();
				foreach (var someItem in LogicParser.GameLogic["Logic"])
				{
					if (!accessible.Contains(someItem.Key))
					{
						if (LogicParser.ParseAccessArray((JArray)someItem.Value, accessible))
						{
							nextSphere.Add(someItem.Key);
						}
					}
				}
			}
			return accessible;
		}
		public static List<Vector3> GetAllAccessibleLocations(HashSet<string> obtainedItems)
		{
			List<Vector3> toReturn = new();
			foreach (var someLocation in CratePlacementsData.BoxPlacements)
			{
				if (LocationsLeftToDistribute.Contains(someLocation.Key))
				{
					//MainLogicLoop.DebugWrite($"Can I access {someLocation.Key}?");
					if (LogicParser.CanAccessLocation(someLocation.Key, obtainedItems))
					{
						toReturn.Add(someLocation.Key);
					}
				}
			}
			return toReturn;
		}
		public static void InitializeWorkingDistribution()
		{
			ItemsLeftToDistribute.Clear();
			LocationsLeftToDistribute.Clear();
			foreach (var someItem in CratePlacementsData.RequiredItems)
			{
				ItemsLeftToDistribute.Add(someItem.Key, new(someItem.Value.HumanReadable, someItem.Value.Count));
			}
			foreach (var someItem in CratePlacementsData.DistributionTable)
			{
				ItemsLeftToDistribute.Add(someItem.Key, new(someItem.Value.HumanReadable, someItem.Value.Count));
			}
			foreach (var someItem in CratePlacementsData.NonStandardDistribution)
			{
				ItemsLeftToDistribute.Add(someItem.Key.ToString(), new(someItem.Key.ToString(), someItem.Value));
			}
			foreach (var someLocation in CratePlacementsData.BoxPlacements)
			{
				LocationsLeftToDistribute.Add(someLocation.Key);
			}
		}
		public static void SetupPlayerStart()
		{
			PluginSetup.CachedRandoData.ObtainedItems.Clear();
			PluginSetup.CachedRandoData.ObtainedItems.Add(LocationTranslation[PluginSetup.CachedRandoData.StartingLocation]);
		}
		public static void PlaceRequiredChestItems()
		{
			foreach (var someRequiredItem in CratePlacementsData.RequiredItems)
			{
				//Location Selection
				int randomKeyIndex = UnityEngine.Random.Range(0, LocationsLeftToDistribute.Count);
				Vector3 locationToPlace = LocationsLeftToDistribute.ElementAt(randomKeyIndex);

				//Item data
				string[] data = { someRequiredItem.Key, someRequiredItem.Value.HumanReadable };

				MainLogicLoop.DebugWrite($"Placing {someRequiredItem.Value.HumanReadable} at {locationToPlace} ({(string)LogicParser.GameLogic["SupplyCrateCoordinates"][locationToPlace.ToString()]["ChestName"]})");

				//Setup and Cleanup
				PluginSetup.CachedRandoData.ChestPlacements.Add(locationToPlace.ToString(), data);
				RequiredLocations.Add(locationToPlace);
				ItemsLeftToDistribute.Remove(someRequiredItem.Key);
				LocationsLeftToDistribute.Remove(locationToPlace);
			}
		}
		public static KeyValuePair<string, Tuple<string, int>> GetRandomItem(ref Dictionary<string, Tuple<string, int>> itemsLeft)
		{
			int randomKeyIndex = UnityEngine.Random.Range(0, itemsLeft.Count);
			KeyValuePair<string, Tuple<string, int>> someItem = itemsLeft.ElementAt(randomKeyIndex);
			if (someItem.Value.Item2 - 1 == 0)
			{
				itemsLeft.Remove(someItem.Key);
			}
			else
			{
				itemsLeft[someItem.Key] = new(someItem.Value.Item1, someItem.Value.Item2 - 1);
			}
			return someItem;
		}
		public static bool CanBeatTheGame()
		{
			foreach (var someRequiredLocation in RequiredLocations)
			{
				HashSet<string> accessibleTokens = GetAllAccessibleTokens(PluginSetup.CachedRandoData.ObtainedItems);
				if (!LogicParser.CanAccessLocation(someRequiredLocation, accessibleTokens))
				{
					return false;
				}
			}
			MainLogicLoop.DebugWrite("You can beat the game!");
			return true;
		}
		public static void PlaceNonRequiredChestItems(int sphere = 0)
		{
			HashSet<string> tempCopyItems = new(PluginSetup.CachedRandoData.ObtainedItems);
			MainLogicLoop.DebugWrite($"Sphere {sphere}.");
			HashSet<string> accessibleTokens = GetAllAccessibleTokens(tempCopyItems);
			string[] sortedTokenArray = accessibleTokens.ToArray();
			Array.Sort(sortedTokenArray);
			MainLogicLoop.DebugWrite($"{String.Join(",", sortedTokenArray)}");
			List<Vector3> accessibleLocations = GetAllAccessibleLocations(accessibleTokens);
			foreach (Vector3 someLocation in accessibleLocations)
			{
				MainLogicLoop.DebugWrite($"\tLocation Accessible {someLocation} ({(string)LogicParser.GameLogic["SupplyCrateCoordinates"][someLocation.ToString()]["ChestName"]}).");
			}
			if (accessibleLocations.Count == 0)
			{
				MainLogicLoop.DebugWrite("No locations for placement! This probably shouldn't ever happen...");
				return;
			}
			bool hasFoundProgression = false;
			int currentLoopCount = 0;
			while (!hasFoundProgression)
			{
				tempCopyItems = new(PluginSetup.CachedRandoData.ObtainedItems);
				Dictionary<Vector3, KeyValuePair<string, Tuple<string, int>>> tempAddedItems = new();
				Dictionary<string, Tuple<string, int>> tempItemsLeft = new(ItemsLeftToDistribute);
				foreach (var someLocation in accessibleLocations)
				{
					var someItem = GetRandomItem(ref tempItemsLeft);
					//MainLogicLoop.DebugWrite($"Trying to place {someItem.Value.Item1} at {someLocation}");
					tempCopyItems.Add(someItem.Value.Item1);
					tempAddedItems.Add(someLocation, someItem);
				}
				HashSet<string> accessibleTokens2 = GetAllAccessibleTokens(tempCopyItems);
				List<Vector3> newAccessibleLocations = GetAllAccessibleLocations(accessibleTokens2);
				if (newAccessibleLocations.Count != accessibleLocations.Count || accessibleLocations.Count == LocationsLeftToDistribute.Count)
				{
					PluginSetup.CachedRandoData.ObtainedItems.UnionWith(tempCopyItems);
					if (accessibleLocations.Count == LocationsLeftToDistribute.Count)
					{
						MainLogicLoop.DebugWrite("\tFinal Placements:");
					}
					else
					{
						MainLogicLoop.DebugWrite("\tProgression Found by the following item set:");
					}
					foreach (var someTempAddedItem in tempAddedItems)
					{
						Vector3 location = someTempAddedItem.Key;
						string classId = someTempAddedItem.Value.Key;
						string humanReadable = someTempAddedItem.Value.Value.Item1;
						int countRemaining = someTempAddedItem.Value.Value.Item2;
						MainLogicLoop.DebugWrite($"\t\t{humanReadable} at {location}.");
						ItemsLeftToDistribute[classId] = new(humanReadable, countRemaining - 1);
						if (ItemsLeftToDistribute[classId].Item2 == 0)
						{
							ItemsLeftToDistribute.Remove(classId);
						}
						string[] data = { classId, humanReadable };
						PluginSetup.CachedRandoData.ChestPlacements[location.ToString()] = data;
						//MainLogicLoop.DebugWrite($"\tDouble Checking {location} at {data[0]}, {data[1]}.");
						LocationsLeftToDistribute.Remove(location);
					}
					hasFoundProgression = true;
				}
				else
				{
					if (currentLoopCount == PluginSetup.Iterations)
					{
						MainLogicLoop.DebugWrite("We've reached our limit, captain.");
						SetupPlayerStart();
						InitializeWorkingDistribution();
						return;
					}
					currentLoopCount++;
				}
			}
			if (LocationsLeftToDistribute.Count == 0)
			{
				return;
			}
			else
			{
				PlaceNonRequiredChestItems(sphere + 1);
			}
		}
		public static void InitializeData()
		{
			SetupPlayerStart();
			InitializeWorkingDistribution();
			PlaceRequiredChestItems();
			while (!CanBeatTheGame())
			{
				PlaceNonRequiredChestItems();
			}
		}
	}
}
