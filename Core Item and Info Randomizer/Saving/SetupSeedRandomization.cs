namespace CoreItemAndInfoRandomizer.Saving
{
	internal class SetupSeedRandomization
	{
		public static void InitializeSeed()
		{
			int toLoadSeed;
			if (PluginSetup.CachedRandoData.SeedData.Item1)
			{
				toLoadSeed = PluginSetup.CachedRandoData.SeedData.Item2;
			}
			else
			{
				PluginSetup.CachedRandoData.SeedData = new(true, PluginSetup.Seed);
				toLoadSeed = PluginSetup.Seed;
			}
			UnityEngine.Random.InitState(toLoadSeed);
		}
	}
}