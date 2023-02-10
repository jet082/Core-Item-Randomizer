using SMLHelper.V2.Handlers;
using System.Collections.Generic;

namespace CoreItemRandomizer
{
	public class PDAEntryOverwrite
	{
		public static void GeneratePDAEntries()
		{
			if (PluginSetup.CachedRandoData.PDAData.Count == 0)
			{
				Dictionary<string, Dictionary<int, string>> junkHintDicts = HintTextData.JunkHints;
				Dictionary<string, string> listOfPdaEntries = HintTextData.PDAEntries;
				Dictionary<string, string> saveJsonCache = new();
				Dictionary<string, string> saveJsonSpoilerLog = new();
				foreach (KeyValuePair<string, string> pdaEntry in listOfPdaEntries)
				{
					//Get a random dictionary from the possible junk hints, then extract the prefix for the title of the database entry
					List<string> junkHintPrefixList = new(junkHintDicts.Keys);
					string randomJunkHintPrefix = junkHintPrefixList[UnityEngine.Random.Range(0, junkHintPrefixList.Count)];
					//Now get (and remove) a random entry from it and set it to a random database entry
					Dictionary<int, string> junkHintTable = junkHintDicts[randomJunkHintPrefix];
					List<int> junkHintNumberList = new(junkHintTable.Keys);
					int randomJunkHintNumber = junkHintNumberList[UnityEngine.Random.Range(0, junkHintNumberList.Count)];
					string newTitle = $"{randomJunkHintPrefix} #{randomJunkHintNumber.ToString().PadLeft(3, '0')}";
					string newDescription = junkHintTable[randomJunkHintNumber];
					saveJsonCache[pdaEntry.Key] = newTitle;
					saveJsonCache[pdaEntry.Value] = newDescription;
					saveJsonSpoilerLog[pdaEntry.Key] = newTitle;
					LanguageHandler.SetLanguageLine(pdaEntry.Key, newTitle);
					LanguageHandler.SetLanguageLine(pdaEntry.Value, newDescription);
					junkHintTable.Remove(randomJunkHintNumber);
					if (junkHintTable.Count == 0)
					{
						junkHintDicts.Remove(randomJunkHintPrefix);
					}
				}
				string[] overwriteCodeKeys = { "Captain's Quarters Code", "Cargo Bay Code", "Robotics Bay Code", "Cabin No.1 Code", "Lab Access Code" };
				List<string> pdaKeyOnlyList = new(listOfPdaEntries.Keys);
				foreach (string someCodeKey in overwriteCodeKeys)
				{
					string finalString = "";
					for (int i = 0; i < 4; i += 1)
					{
						finalString += UnityEngine.Random.Range(0, 9).ToString();
					}
					MainLogicLoop.DebugWrite($"Setting {someCodeKey} to {finalString}");
					int toOverwriteIndex = UnityEngine.Random.Range(0, pdaKeyOnlyList.Count);
					string toBeCodeTitle = pdaKeyOnlyList[toOverwriteIndex];
					string toBeCodeDescription = listOfPdaEntries[toBeCodeTitle];
					saveJsonCache[toBeCodeTitle] = someCodeKey;
					saveJsonCache[toBeCodeDescription] = $"The {someCodeKey} is {finalString}.";
					saveJsonSpoilerLog[toBeCodeTitle] = someCodeKey;
					LanguageHandler.SetLanguageLine(toBeCodeTitle, someCodeKey);
					LanguageHandler.SetLanguageLine(toBeCodeDescription, $"The {someCodeKey} is {finalString}.");
					PluginSetup.CachedRandoData.Codes[someCodeKey] = finalString;
					PluginSetup.SpoilerLogData.Codes[someCodeKey] = finalString;
				}
				PluginSetup.CachedRandoData.PDAData = saveJsonCache;
				PluginSetup.SpoilerLogData.PDAData = saveJsonSpoilerLog;
			}
			else
			{
				foreach (KeyValuePair<string, string> pdaEntry in PluginSetup.SpoilerLogData.PDAData)
				{
					LanguageHandler.SetLanguageLine(pdaEntry.Key, pdaEntry.Value);
				}
			}
		}
	}
}