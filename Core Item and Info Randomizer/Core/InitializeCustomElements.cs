namespace CoreItemAndInfoRandomizer.Mod_Items
{
	internal class InitializeCustomElements
	{
		public static void InitializeDolls()
		{
			new MyVeryOwnSupplyCrate().Patch();
			new RandoSeamothDoll().Patch();
			new RandoRocketBaseDoll().Patch();
			new RandoCyclopsDoll().Patch();
			new RandoPrawnSuitDoll().Patch();
			new RandoRocketBaseLadderDoll().Patch();
			new RandoRocketStage1Doll().Patch();
			new RandoRocketStage2Doll().Patch();
			new RandoRocketStage3Doll().Patch();
		}
		public static void InitializeCustomCreatures()
		{
			new AmyThePeeperLeviathan().Patch();
		}
		public static void Initialize()
		{
			InitializeDolls();
			InitializeCustomCreatures();
			ModCache.Setup();
		}
	}
}
