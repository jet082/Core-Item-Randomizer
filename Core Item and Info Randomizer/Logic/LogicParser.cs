using Newtonsoft.Json.Linq;

namespace CoreItemAndInfoRandomizer.Logic
{
	public class LogicParser
	{
		public static bool ParseAccessArray(JArray someArray, bool isOr = true)
		{
			SaveData saveData = PluginSetup.RandomizerLoadedSaveData;
			foreach (JToken rootLogicToken in someArray)
			{
				if (rootLogicToken.Type == JTokenType.Array)
				{
					bool result = ParseAccessArray((JArray)rootLogicToken, !isOr);
					if (isOr && result)
					{
						return true;
					}
					else if (!isOr && !result)
					{
						return false;
					}
				}
				else if (rootLogicToken.Type == JTokenType.String)
				{
					string toCheck = rootLogicToken.Value<string>();
					if (PluginSetup.DebugMode)
					{
						PluginSetup.BepinExLogger.LogInfo($"Checking {toCheck}");
					}
					if (saveData.ObtainedItems.Contains(toCheck))
					{
						if (isOr)
						{
							return true;
						}
					}
					else if (MainLogicLoop.GameLogic["logic"].ContainsKey(toCheck))
					{
						bool result = CanAccess(toCheck);
						if (isOr && result)
						{
							return true;
						}
						else if (!isOr && !result)
						{
							return false;
						}
					}
					else
					{
						if (PluginSetup.DebugMode)
						{
							PluginSetup.BepinExLogger.LogInfo("We reached a dead end with no item found.");
						}
						if (!isOr)
						{
							return false;
						}
					}
				}
				else
				{
					PluginSetup.BepinExLogger.LogInfo($"We should have not gotten here in the Logic Parser. The type was {rootLogicToken.Type}");
				}
			}
			if (!isOr)
			{
				return true;
			}
			return false;
		}
		public static bool CanAccess(string someCheck)
		{
			JArray rootLogic = (JArray)MainLogicLoop.GameLogic["logic"][someCheck];
			return ParseAccessArray(rootLogic);
		}
	}
}