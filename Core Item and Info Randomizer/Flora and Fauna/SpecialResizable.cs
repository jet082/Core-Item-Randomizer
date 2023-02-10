using UnityEngine;

namespace CoreItemRandomizer
{
	public class SpecialResizable : MonoBehaviour
	{
		public float ScaleFactor = 1f;
		public void Start()
		{
			gameObject.GetComponentInParent<Creature>().SetScale(ScaleFactor);
		}
	}
}
