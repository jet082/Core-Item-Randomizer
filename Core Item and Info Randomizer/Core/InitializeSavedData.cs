using CoreItemAndInfoRandomizer.Creatures;

namespace CoreItemAndInfoRandomizer.Core
{
	internal class InitializeSavedData
	{
		public static void Initialize()
		{
			PDAPatcher.GeneratePDAEntries();
			RandomizeFishSpecies.Randomize();
		}
	}
}