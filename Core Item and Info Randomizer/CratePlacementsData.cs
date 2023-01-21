using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class CratePlacementsData
	{
		public static Dictionary<TechType, int> DistributionTable = new();
		public static Dictionary<TechType, int> RequiredItems = new();
		public static Dictionary<Vector3, JToken> BoxPlacements;
		public static void Setup()
		{
			DistributionTable = new() {
				{ TechType.HatchingEnzymes, 1},
				{ TechType.Tank, 1 },
				{ TechType.DoubleTank, 1 },
				{ TechType.PlasteelTank, 1 },
				{ TechType.HighCapacityTank, 1 },
				{ TechType.Compass, 1 },
				{ TechType.FireExtinguisher, 1 },
				{ TechType.FirstAidKit, 1 },
				{ TechType.Rebreather, 1 },
				{ TechType.RadiationGloves, 1 },
				{ TechType.RadiationHelmet, 1 },
				{ TechType.RadiationSuit, 1 },
				{ TechType.ReinforcedDiveSuit, 1 },
				{ TechType.ReinforcedGloves, 1 },
				{ TechType.WaterFiltrationSuit, 1 },
				{ TechType.Fins, 1 },
				{ TechType.SwimChargeFins, 1 },
				{ TechType.UltraGlideFins, 1 },
				{ TechType.PrecursorKey_Purple, 1 },
				{ TechType.PrecursorKey_Orange, 1 },
				{ TechType.PrecursorKey_Blue, 1 },
				{ TechType.PipeSurfaceFloater, 1 },
				{ TechType.Pipe, 1 },
				{ TechType.MapRoomHUDChip, 1 },
				{ TechType.Coffee, 1 },
				{ TechType.AirBladder, 1 },
				{ TechType.Flare, 1 },
				{ TechType.Flashlight, 1 },
				{ TechType.Builder, 1 },
				{ TechType.LaserCutter, 1 },
				{ TechType.LEDLight, 1 },
				{ TechType.DiveReel, 1 },
				{ TechType.PropulsionCannon, 1 },
				{ TechType.Welder, 1 },
				{ TechType.RepulsionCannon, 1 },
				{ TechType.Scanner, 1 },	
				{ TechType.Seaglide, 1 },
				{ TechType.StasisRifle, 1 },
				{ TechType.Knife, 1 },
				{ TechType.HeatBlade, 1 },
				{ TechType.CyclopsDecoyModule, 1 },
				{ TechType.CyclopsDecoy, 1 },
				{ TechType.CyclopsHullModule1, 1 },
				{ TechType.CyclopsHullModule2, 1 },
				{ TechType.CyclopsHullModule3, 1 },
				{ TechType.CyclopsSeamothRepairModule, 1 },
				{ TechType.PowerUpgradeModule, 1 },
				{ TechType.CyclopsShieldModule, 1 },
				{ TechType.CyclopsSonarModule, 1 },
				{ TechType.CyclopsFireSuppressionModule, 1 },
				{ TechType.CyclopsThermalReactorModule, 1 },
				{ TechType.VehiclePowerUpgradeModule, 1 },
				{ TechType.ExosuitPropulsionArmModule, 1 },
				{ TechType.ExosuitTorpedoArmModule, 1 },
				{ TechType.ExosuitGrapplingArmModule, 1 },
				{ TechType.ExosuitDrillArmModule, 1 },
				{ TechType.ExosuitJetUpgradeModule, 1 },
				{ TechType.ExosuitThermalReactorModule, 1 },
				{ TechType.VehicleArmorPlating, 1 },
				{ TechType.ExoHullModule1, 1 },
				{ TechType.ExoHullModule2, 1 },
				{ TechType.VehicleStorageModule, 1 },
				{ TechType.GasTorpedo, 1 },
				{ TechType.WhirlpoolTorpedo, 1 },
				{ TechType.HullReinforcementModule, 1 },
				{ TechType.HullReinforcementModule2, 1 },
				{ TechType.HullReinforcementModule3, 1 },
				{ TechType.SeamothSolarCharge, 1 },
				{ TechType.SeamothElectricalDefense, 1 },
				{ TechType.SeamothTorpedoModule, 1 },
				{ TechType.SeamothSonarModule, 1 },
				{ TechType.MapRoomUpgradeScanRange, 4 },
				{ TechType.MapRoomUpgradeScanSpeed, 4 },
				{ TechType.MapRoomCamera, 2 },
				{ TechType.PrecursorIonCrystal, 10 },
				{ TechType.SeaTreaderPoop, 3 },
				{ ModCache.CacheData["RandoSeamothDoll"].ModTechType, 1 },
				{ ModCache.CacheData["RandoPrawnSuitDoll"].ModTechType, 1 },
				{ ModCache.CacheData["RandoCyclopsDoll"].ModTechType, 1 }
			};
			RequiredItems = new()
			{
				{ TechType.EnzymeCureBall, 1 },
				{ ModCache.CacheData["RandoRocketBaseDoll"].ModTechType, 1 },
				{ ModCache.CacheData["RandoRocketBaseLadderDoll"].ModTechType, 1 },
				{ ModCache.CacheData["RandoRocketStage1Doll"].ModTechType, 1 },
				{ ModCache.CacheData["RandoRocketStage2Doll"].ModTechType, 1 },
				{ ModCache.CacheData["RandoRocketStage3Doll"].ModTechType, 1 }
			};
		}
		public static Dictionary<Vector3, JToken> BoxPlacementDictionary()
		{
			IDictionary<string, JToken> tempDictData = (IDictionary<string, JToken>)MainLogicLoop.GameLogic["supplyCrateCoordinates"];
			Dictionary<Vector3, JToken> finalDict = new();
			foreach (KeyValuePair<string, JToken> someData in tempDictData)
			{
				string[] vectorData = someData.Key.Split(',');
				Vector3 vectorized = new(float.Parse(vectorData[0]), float.Parse(vectorData[1]), float.Parse(vectorData[2]));
				finalDict.Add(vectorized, someData.Value);
			}
			BoxPlacements = finalDict;
			return finalDict;
		}
	}
}