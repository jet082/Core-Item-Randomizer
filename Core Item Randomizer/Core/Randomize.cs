using CoreItemRandomizer.FloraAndFauna;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CoreItemRandomizer.Core
{
	internal class Randomize
	{
		public static readonly Dictionary<Vector3, string> StartingLocations = new()
		{
			{ new Vector3(-110.6f, 0f, -347.1f), "The Safe Shallows" },
			{ new Vector3(-728.0f, -761f, -274.7f), "Lost River Skull" },
			{ new Vector3(401.2f, 0f, -524.1f), "Crash Zone Trench" },
			{ new Vector3(1001.9f, 0, -1120.3f), "Crash Zone SE" }
		};
		public static void RandomizeStartingLocation()
		{
			if (PluginSetup.RandomStartingLocation)
			{
				if (PluginSetup.CachedRandoData.StartingLocation.Equals(Vector3.zero))
				{
					int selectionNumber = UnityEngine.Random.Range(0, StartingLocations.Count);
					PluginSetup.CachedRandoData.StartingLocation = StartingLocations.ElementAt(selectionNumber).Key;
					PluginSetup.SpoilerLogData.StartingLocation = StartingLocations.ElementAt(selectionNumber).Value;
				}
			}
		}
		public static void Initialize()
		{
			PDAEntryOverwrite.GeneratePDAEntries();
			RandomizeSizes.Randomize();
			RandomizeStartingLocation();
		}
	}
}