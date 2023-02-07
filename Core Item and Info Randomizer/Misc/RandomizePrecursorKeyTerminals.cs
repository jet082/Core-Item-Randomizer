using HarmonyLib;
using SMLHelper.V2.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace CoreItemAndInfoRandomizer.Assorted_Patches
{
	[HarmonyPatch]
	public class RandomizePrecursorKeyTerminals
	{
		public static Dictionary<PrecursorKeyTerminal.PrecursorKeyType, string> KeyTextureTranslation = new()
		{
			{ PrecursorKeyTerminal.PrecursorKeyType.PrecursorKey_Red, "Precursor_Symbol01.png" },
			{ PrecursorKeyTerminal.PrecursorKeyType.PrecursorKey_Orange, "Precursor_Symbol02.png" },
			{ PrecursorKeyTerminal.PrecursorKeyType.PrecursorKey_Blue, "Precursor_Symbol03.png" },
			{ PrecursorKeyTerminal.PrecursorKeyType.PrecursorKey_White, "Precursor_Symbol04.png" },
			{ PrecursorKeyTerminal.PrecursorKeyType.PrecursorKey_Purple, "Precursor_Symbol05.png" },
		};
		public static Dictionary<Vector3, string> ListOfTerminals = new()
		{
			{ new Vector3(448.8f, -93.9f, 1219.6f), "Quarantine Enforcement Platform Moonpool Outside" },
			{ new Vector3(437.8f, -95.5f, 1222.9f), "Quarantine Enforcement Platform Moonpool Inside" },
			{ new Vector3(384.9f, 3.0f, 1112.1f), "Quarantine Enforcement Platform Main Entrance Outside" },
			{ new Vector3(391.6f, 3.0f, 1106.6f), "Quarantine Enforcement Platform Main Entrance Outside" },
			{ new Vector3(402.4f, -74.3f, 1096.8f), "Quarantine Enforcement Platform Control Room" },
			{ new Vector3(-64.4f, -1227.2f, 106.8f), "Thermal Plant Generator Room" },
			{ new Vector3(-63.7f, -1212.4f, 119.2f), "Thermal Plant Blue Tablet Room" },
			{ new Vector3(-279.1f, -802.1f, 293.4f), "Disease Research Facility" },
			{ new Vector3(-607.8f, -562.6f, 1478.5f), "Blood Kelp Zone Sanctuary Cache" },
			{ new Vector3(-880.8f, -312.2f, -799.3f), "Deep Sparse Reef Sanctuary Cache" },
			{ new Vector3(-1251.2f, -400.6f, 1099.8f), "Dunes Sanctuary Cache" },
			{ new Vector3(-1107.7f, -689.3f, -663.2f), "Laboratory Cache" },
			{ new Vector3(218.7f, -1453.2f, -272.3f), "Primary Containment Facility Entrance" },
			{ new Vector3(276.4f, -1438.2f, -366.8f), "Primary Containment Facility Moonpool" },
		};
		[HarmonyPatch(typeof(PrecursorKeyTerminal))]
		[HarmonyPatch(nameof(PrecursorKeyTerminal.Start))]
		[HarmonyPostfix]
		public static void AssignRandomKey(PrecursorKeyTerminal __instance)
		{
			Array values = Enum.GetValues(typeof(PrecursorKeyTerminal.PrecursorKeyType));
			PrecursorKeyTerminal.PrecursorKeyType newKey = (PrecursorKeyTerminal.PrecursorKeyType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
			Texture newTexture = ImageUtils.LoadTextureFromFile(Path.Combine(SaveAndLoad.PluginPath, "Assets", KeyTextureTranslation[newKey]));
			__instance.transform.Find("Precursor_key_terminal_01/glyph/Face_F").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
			__instance.acceptKeyType = newKey;
		}
	}
}