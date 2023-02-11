using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemRandomizer.Logic
{
	public class LogicParser
	{
		public static string SuccessMessage = "CONDITION MET!";
		public static string FailureMessage = "CONDITION NOT MET!";
		public static Dictionary<string, JObject> GameLogic = SaveAndLoad.LoadLogic("DefaultLogic.json");
		public static bool ParseAccessArray(JArray someArray, HashSet<string> obtainedItems, bool isOr = true, int iteration = 0)
		{
			/*string toAppendDebugString;
			if (isOr)
			{
				toAppendDebugString = "or";
			}
			else
			{
				toAppendDebugString = "and";
			}*/
			string tabs = "";
			for (int i = 0; i <= iteration; i++)
			{
				tabs += "\t";
			}
			foreach (JToken rootLogicToken in someArray)
			{
				if (rootLogicToken.Type == JTokenType.Array)
				{
					//MainLogicLoop.DebugWrite($"{tabs}SERIES OF CONDITIONS BEGINS ({toAppendDebugString})");
					bool result = ParseAccessArray((JArray)rootLogicToken, obtainedItems, !isOr, iteration + 1);
					//MainLogicLoop.DebugWrite($"{tabs}SERIES OF CONDITIONS ENDS ({toAppendDebugString})");
					if (isOr && result)
					{
						//MainLogicLoop.DebugWrite($"{tabs}{SuccessMessage}");
						return true;
					}
					else if (!isOr && !result)
					{
						//MainLogicLoop.DebugWrite($"{tabs}{SuccessMessage}");
						return false;
					}
				}
				else if (rootLogicToken.Type == JTokenType.String)
				{
					string toCheck = (string)rootLogicToken;
					if (obtainedItems.Contains(toCheck))
					{
						//MainLogicLoop.DebugWrite($"{tabs}{toCheck} {toAppendDebugString}");
						//MainLogicLoop.DebugWrite($"{tabs}\tFOUND");
						if (isOr)
						{
							//MainLogicLoop.DebugWrite($"{tabs}{SuccessMessage}");
							return true;
						}
					}
					else
					{
						//MainLogicLoop.DebugWrite($"{tabs}{toCheck} {toAppendDebugString}");
						//MainLogicLoop.DebugWrite($"{tabs}\tNOT FOUND");
						if (!isOr)
						{
							//MainLogicLoop.DebugWrite($"{tabs}{FailureMessage}");
							return false;
						}
					}
				}
				else
				{
					//MainLogicLoop.DebugWrite($"{tabs}\tWe should have not gotten here in the Logic Parser. The type was {rootLogicToken.Type}.");
				}
			}
			if (!isOr)
			{
				//MainLogicLoop.DebugWrite($"{tabs}{SuccessMessage}");
				return true;
			}
			//MainLogicLoop.DebugWrite($"{tabs}{FailureMessage}");
			return false;
		}
		public static bool CanAccessLocation(Vector3 someVector, HashSet<string> obtainedItems)
		{
			JArray toProcessLocation = (JArray)GameLogic["SupplyCrateCoordinates"][someVector.ToString()]["Logic"];
			return ParseAccessArray(toProcessLocation, obtainedItems);
		}
	}
}