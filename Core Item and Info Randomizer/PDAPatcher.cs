using SMLHelper.V2.Handlers;
using System.Collections.Generic;
using System;
using System.Text.Json;
using System.Xml;
using System.IO;

namespace CoreItemAndInfoRandomizer
{
	public class PDAPatcher
	{
		public static string PDAData = Path.Combine(SaveAndLoad.SavePath, "RandomizerPDASaveData.json");
		public static void ToSavePDAData(Dictionary<string, string> toSaveDict) {
			using FileStream openStream = File.OpenRead(PDAData);
			JsonSerializer.Serialize(openStream, toSaveDict, new JsonSerializerOptions { WriteIndented = true });
		}
		public static void GeneratePDAEntries()
		{
			if (!File.Exists(PDAData)) {
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
				ToSavePDAData(saveJson);
			} else
			{
				string json = File.ReadAllText(PDAData);
				Dictionary<string, string>toLoadPdaData = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
				foreach (KeyValuePair<string, string> pdaEntry in toLoadPdaData)
				{
					LanguageHandler.SetLanguageLine(pdaEntry.Key, pdaEntry.Value);
				}
			}
		}
	}
}