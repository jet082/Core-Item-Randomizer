﻿using SMLHelper.V2.Handlers;
using System.Collections.Generic;
using System;
using SMLHelper.V2.Json;

namespace CoreItemAndInfoRandomizer
{
	public class PDAPatcher
	{
		public static void GeneratePDAEntries()
		{

			var pdaSaveData = PluginSetup.randomizerSaveData;
			pdaSaveData.OnFinishedLoading += (object sender, JsonFileEventArgs e) =>
			{
				SaveData pdaLoadData = e.Instance as SaveData;
				if (pdaSaveData.PDAData.Count == 0)
				{
					Dictionary<string, Dictionary<int, string>> junkHintDicts = HintTextData.JunkHints;
					Dictionary<string, string> listOfPdaEntries = HintTextData.PDAEntries;
					Dictionary<string, string> saveJson = new();
					Random rand = new();
					foreach (KeyValuePair<string, string> pdaEntry in listOfPdaEntries)
					{
						//Get a random dictionary from the possible junk hints, then extract the prefix for the title of the database entry
						List<string> junkHintPrefixList = new(junkHintDicts.Keys);
						string randomJunkHintPrefix = junkHintPrefixList[rand.Next(junkHintPrefixList.Count)];
						//Now get (and remove) a random entry from it and set it to a random database entry
						Dictionary<int, string> junkHintTable = junkHintDicts[randomJunkHintPrefix];
						List<int> junkHintNumberList = new(junkHintTable.Keys);
						int randomJunkHintNumber = junkHintNumberList[rand.Next(junkHintNumberList.Count)];
						string newTitle = $"{randomJunkHintPrefix} #{randomJunkHintNumber.ToString().PadLeft(3, '0')}";
						string newDescription = junkHintTable[randomJunkHintNumber];
						saveJson[pdaEntry.Key] = newTitle;
						saveJson[pdaEntry.Value] = newDescription;
						LanguageHandler.SetLanguageLine(pdaEntry.Key, newTitle);
						LanguageHandler.SetLanguageLine(pdaEntry.Value, newDescription);
						junkHintTable.Remove(randomJunkHintNumber);
						if (junkHintTable.Count == 0)
						{
							junkHintDicts.Remove(randomJunkHintPrefix);
						}
					}
					pdaSaveData.PDAData = saveJson;
				}
				else
				{
					foreach (KeyValuePair<string, string> pdaEntry in pdaLoadData.PDAData)
					{
						LanguageHandler.SetLanguageLine(pdaEntry.Key, pdaEntry.Value);
					}
				}
			};
		}
	}
}