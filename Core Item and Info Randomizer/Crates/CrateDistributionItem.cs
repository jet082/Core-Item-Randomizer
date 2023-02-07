namespace CoreItemAndInfoRandomizer.Crates
{
	public class CrateDistributionItem
	{
		public int Count = 1;
		public string HumanReadable = "";
		public CrateDistributionItem(int count = 1, string humanReadable = "")
		{
			Count = count;
			HumanReadable = humanReadable;
		}
	}
}
