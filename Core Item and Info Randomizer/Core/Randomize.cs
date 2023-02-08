using CoreItemAndInfoRandomizer.FloraAndFauna;

namespace CoreItemAndInfoRandomizer.Core
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