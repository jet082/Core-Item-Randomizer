using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CoreItemAndInfoRandomizer
{
	class LogicParser
	{
		readonly JObject logic;
		public void RunTheShow()
		{
			string json = File.ReadAllText("DefaultLogic.json");
			LogicParser parser = new(json);
			var possibilities = parser.Parse("vehicle");
			foreach (var item in possibilities)
			{
				Console.WriteLine(string.Join(",", item));
			}
		}
		public LogicParser(string json)
		{
			logic = JObject.Parse(json);
		}

		public List<string[]> Parse(string key)
		{
			var possibilities = new List<string[]>();
			ParseHelper(key, possibilities);
			return possibilities;
		}

		private void ParseHelper(string key, List<string[]> possibilities)
		{
			if (!logic.ContainsKey(key))
			{
				return;
			}
			var value = logic[key];
			if (value.Type == JTokenType.String)
			{
				possibilities.Add(new string[] { value.ToString() });
			}
			else if (value.Type == JTokenType.Array)
			{
				var orValues = new List<string[]>();
				foreach (var v in value)
				{
					if (v.Type == JTokenType.String)
					{
						orValues.Add(new string[] { v.ToString() });
					}
					else if (v.Type == JTokenType.Array)
					{
						var andValues = new List<string>();
						foreach (var v2 in v)
						{
							if (v2.Type == JTokenType.String)
							{
								andValues.Add(v2.ToString());
							}
							else if (v2.Type == JTokenType.Array)
							{
								ParseHelper(v2.ToString(), orValues);
							}
						}
						if (andValues.Count > 0)
						{
							orValues.Add(andValues.ToArray());
						}
					}
				}
				var finalList = new List<string[]>();
				if (orValues.Count > 0)
				{
					foreach (var array1 in orValues)
					{
						if (finalList.Count == 0)
						{
							finalList.AddRange(array1.Select(item => new string[] { item }));
						}
						else
						{
							var newList = new List<string[]>();
							foreach (var array2 in finalList)
							{
								foreach (var item in array1)
								{
									newList.Add(ConcatenateArrays(array2, new string[] { item }));
								}
							}
							finalList = newList;
						}
					}
					possibilities.AddRange(finalList);
				}
			}
		}

		private string[] ConcatenateArrays(string[] a, IEnumerable<string> b)
		{
			var c = new string[a.Length + b.Count()];
			a.CopyTo(c, 0);
			b.ToArray().CopyTo(c, a.Length);
			return c;
		}
	}
}