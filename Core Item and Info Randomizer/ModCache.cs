﻿using SMLHelper.V2.Handlers;
using System.Collections.Generic;

namespace CoreItemAndInfoRandomizer
{
	public class ModCache
	{
		public static Dictionary<string, ModCacheItem> CacheData = new() {
			{ "RandoSeamothDoll", new ModCacheItem() },
			{ "RandoPrawnSuitDoll", new ModCacheItem() },
			{ "RandoCyclopsDoll", new ModCacheItem() },
			{ "RandoRocketBaseDoll", new ModCacheItem() }
		};
		public static TechType RandoSeamothDollCache;
		public static TechType RandoPrawnSuitDollCache;
		public static TechType RandoCyclopsDollCache;
		public static TechType RandoRocketBaseDollCache;
		public static void Setup()
		{
			foreach(string someModItem in CacheData.Keys)
			{
				TechType someTempTechType;
				_ = TechTypeHandler.TryGetModdedTechType(someModItem, out someTempTechType);
				string someTempClassId = CraftData.GetClassIdForTechType(someTempTechType);
				CacheData[someModItem].ModTechType = someTempTechType;
				CacheData[someModItem].ClassId = someTempClassId;
			}
		}
	}
}