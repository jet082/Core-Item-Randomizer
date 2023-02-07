using CoreItemAndInfoRandomizer.Crates;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class CratePlacementsData
	{
		public static Dictionary<string, CrateDistributionItem> DistributionTable = new();
		public static Dictionary<string, CrateDistributionItem> RequiredItems = new();
		public static Dictionary<Vector3, JToken> BoxPlacements;
		public static TechType[] ArrayOfNonRequiredTechTypes = {
			TechType.HatchingEnzymes,
			TechType.Tank,
			TechType.DoubleTank,
			TechType.PlasteelTank,
			TechType.HighCapacityTank,
			TechType.Compass,
			TechType.FireExtinguisher,
			TechType.FirstAidKit,
			TechType.Rebreather,
			TechType.RadiationGloves,
			TechType.RadiationHelmet,
			TechType.RadiationSuit,
			TechType.ReinforcedDiveSuit,
			TechType.ReinforcedGloves,
			TechType.WaterFiltrationSuit,
			TechType.Fins,
			TechType.SwimChargeFins,
			TechType.UltraGlideFins,
			TechType.PrecursorKey_Purple,
			TechType.PrecursorKey_Orange,
			TechType.PrecursorKey_Blue,
			TechType.PipeSurfaceFloater,
			TechType.Pipe,
			TechType.MapRoomHUDChip,
			TechType.Coffee,
			TechType.AirBladder,
			TechType.Flare,
			TechType.Flashlight,
			TechType.Builder,
			TechType.LaserCutter,
			TechType.LEDLight,
			TechType.DiveReel,
			TechType.PropulsionCannon,
			TechType.Welder,
			TechType.RepulsionCannon,
			TechType.Scanner,
			TechType.Seaglide,
			TechType.StasisRifle,
			TechType.Knife,
			TechType.HeatBlade,
			TechType.CyclopsDecoyModule,
			TechType.CyclopsDecoy,
			TechType.CyclopsSeamothRepairModule,
			TechType.PowerUpgradeModule,
			TechType.CyclopsShieldModule,
			TechType.CyclopsSonarModule,
			TechType.CyclopsFireSuppressionModule,
			TechType.CyclopsThermalReactorModule,
			TechType.VehiclePowerUpgradeModule,
			TechType.ExosuitPropulsionArmModule,
			TechType.ExosuitTorpedoArmModule,
			TechType.ExosuitGrapplingArmModule,
			TechType.ExosuitDrillArmModule,
			TechType.ExosuitJetUpgradeModule,
			TechType.ExosuitThermalReactorModule,
			TechType.VehicleArmorPlating,
			TechType.VehicleStorageModule,
			TechType.GasTorpedo,
			TechType.WhirlpoolTorpedo,
			TechType.CyclopsHullModule1,
			TechType.CyclopsHullModule2,
			TechType.CyclopsHullModule3,
			TechType.ExoHullModule1,
			TechType.ExoHullModule2,
			TechType.VehicleHullModule1,
			TechType.VehicleHullModule2,
			TechType.VehicleHullModule3,
			TechType.SeamothSolarCharge,
			TechType.SeamothElectricalDefense,
			TechType.SeamothTorpedoModule,
			TechType.SeamothSonarModule,
			TechType.MapRoomUpgradeScanRange,
			TechType.MapRoomUpgradeScanSpeed,
			TechType.MapRoomCamera,
			TechType.PrecursorIonCrystal,
			TechType.SeaTreaderPoop
		};
		public static Dictionary<TechType, int> NonStandardDistribution = new() {
			{ TechType.MapRoomUpgradeScanRange, 4 },
			{ TechType.MapRoomUpgradeScanSpeed, 4 },
			{ TechType.MapRoomCamera, 2 },
			{ TechType.PrecursorIonCrystal, 10 },
			{ TechType.SeaTreaderPoop, 3 }
		};
		public static void Setup()
		{
			BoxPlacements = BoxPlacementDictionary();
			foreach (TechType someTechType in ArrayOfNonRequiredTechTypes)
			{
				if (NonStandardDistribution.ContainsKey(someTechType))
				{
					DistributionTable.Add(CraftData.GetClassIdForTechType(someTechType), new(NonStandardDistribution[someTechType], Language.main.Get(someTechType)));
				}
				else
				{
					DistributionTable.Add(CraftData.GetClassIdForTechType(someTechType), new(1, Language.main.Get(someTechType)));
				}
			}
			DistributionTable.Add(ModCache.CacheData["RandoSeamothDoll"].ClassId, new(1, "Seamoth"));
			DistributionTable.Add(ModCache.CacheData["RandoPrawnSuitDoll"].ClassId, new(1, "Prawn Suit"));
			DistributionTable.Add(ModCache.CacheData["RandoCyclopsDoll"].ClassId, new(1, "Cyclops"));
			RequiredItems.Add(CraftData.GetClassIdForTechType(TechType.EnzymeCureBall), new(1, Language.main.Get(TechType.EnzymeCureBall)));
			RequiredItems.Add(ModCache.CacheData["RandoRocketBaseDoll"].ClassId, new(1, Language.main.Get("Neptune Launch Platform")));
			RequiredItems.Add(ModCache.CacheData["RandoRocketBaseLadderDoll"].ClassId, new(1, Language.main.Get("Neptune Gantry")));
			RequiredItems.Add(ModCache.CacheData["RandoRocketStage1Doll"].ClassId, new(1, Language.main.Get("Neptune Ion Boosters")));
			RequiredItems.Add(ModCache.CacheData["RandoRocketStage2Doll"].ClassId, new(1, Language.main.Get("Neptune Fuel Reserve")));
			RequiredItems.Add(ModCache.CacheData["RandoRocketStage3Doll"].ClassId, new(1, Language.main.Get("Neptune Cockpit")));
		}
		public static Dictionary<Vector3, JToken> BoxPlacementDictionary()
		{
			Dictionary<Vector3, JToken> finalDict = new();
			foreach (var someData in MainLogicLoop.GameLogic["supplyCrateCoordinates"])
			{
				string[] vectorData = someData.Key.Replace(" ", "").Split(',');
				Vector3 vectorized = new(float.Parse(vectorData[0]), float.Parse(vectorData[1]), float.Parse(vectorData[2]));
				finalDict.Add(vectorized, someData.Value);
			}
			return finalDict;
		}
	}
}