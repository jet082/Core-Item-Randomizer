namespace CoreItemAndInfoRandomizer
{
	public class ModCacheItem
	{
		public TechType ModTechType { get; set; }
		public string ClassId { get; set; }
		public ModCacheItem()
		{
			ModTechType = TechType.None;
			ClassId = string.Empty;
		}
	}
}