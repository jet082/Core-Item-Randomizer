using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class CratePlacementsData
	{
		public static Dictionary<string, int> DistributionTable = new();
		public static Dictionary<string, int> RequiredItems = new();
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
			TechType.Scanner    ,
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
			DistributionTable = new() {
				{ ModCache.CacheData["RandoSeamothDoll"].ClassId, 1 },
				{ ModCache.CacheData["RandoPrawnSuitDoll"].ClassId, 1 },
				{ ModCache.CacheData["RandoCyclopsDoll"].ClassId, 1 }
			};
			foreach (TechType someTechType in ArrayOfNonRequiredTechTypes)
			{
				if (NonStandardDistribution.ContainsKey(someTechType))
				{
					DistributionTable[CraftData.GetClassIdForTechType(someTechType)] = NonStandardDistribution[someTechType];
				}
				else
				{
					DistributionTable[CraftData.GetClassIdForTechType(someTechType)] = 1;
				}
			}
			RequiredItems = new()
			{
				{ CraftData.GetClassIdForTechType(TechType.EnzymeCureBall), 1 },
				{ ModCache.CacheData["RandoRocketBaseDoll"].ClassId, 1 },
				{ ModCache.CacheData["RandoRocketBaseLadderDoll"].ClassId, 1 },
				{ ModCache.CacheData["RandoRocketStage1Doll"].ClassId, 1 },
				{ ModCache.CacheData["RandoRocketStage2Doll"].ClassId, 1 },
				{ ModCache.CacheData["RandoRocketStage3Doll"].ClassId, 1 }
			};
		}
		public static Dictionary<Vector3, JToken> BoxPlacementDictionary()
		{
			Dictionary<Vector3, JToken> finalDict = new();
			foreach (var someData in MainLogicLoop.GameLogic["supplyCrateCoordinates"])
			{
				string[] vectorData = someData.Key.Split(',');
				Vector3 vectorized = new(float.Parse(vectorData[0]), float.Parse(vectorData[1]), float.Parse(vectorData[2]));
				finalDict.Add(vectorized, someData.Value);
			}
			return finalDict;
		}
	}
}