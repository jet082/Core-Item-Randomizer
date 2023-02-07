using HarmonyLib;
using System;

namespace CoreItemAndInfoRandomizer.Assorted_Patches
{
	[HarmonyPatch]
	public class RandomizePrecursorKeyTerminals
	{
		[HarmonyPatch(typeof(PrecursorKeyTerminal))]
		[HarmonyPatch(nameof(PrecursorKeyTerminal.Start))]
		[HarmonyPostfix]
		public static void AssignRandomKey(PrecursorKeyTerminal __instance)
		{
			MainLogicLoop.DebugWrite("Woaaah");
			Array values = Enum.GetValues(typeof(PrecursorKeyTerminal.PrecursorKeyType));
			PrecursorKeyTerminal.PrecursorKeyType newKey = (PrecursorKeyTerminal.PrecursorKeyType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
			__instance.acceptKeyType = newKey;
		}
	}
}