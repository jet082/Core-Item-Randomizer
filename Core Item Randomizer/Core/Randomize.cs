using CoreItemRandomizer.FloraAndFauna;

namespace CoreItemRandomizer.Core
{
	internal class Randomize
	{
		public static void Initialize()
		{
			PDAEntryOverwrite.GeneratePDAEntries();
			RandomizeSizes.Randomize();
		}
	}
}