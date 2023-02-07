using CoreItemAndInfoRandomizer.Creatures;

namespace CoreItemAndInfoRandomizer.Core
{
	internal class InitializeSavedData
	{
		public static void Initialize()
		{
			PDAEntryOverwrite.GeneratePDAEntries();
			RandomizeFishSpecies.Randomize();
			//PrecursorKeyTerminal.PrecursorKeyType heresHowYouGetThis = PrecursorKeyTerminal.PrecursorKeyType.PrecursorKey_Red;
		}
	}
}