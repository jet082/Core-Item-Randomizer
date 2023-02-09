using SMLHelper.V2.Assets;
using System.Collections;
using UnityEngine;
using UWE;

namespace CoreItemAndInfoRandomizer
{
	public class MyVeryOwnSupplyCrate : Spawnable
	{
		public MyVeryOwnSupplyCrate() : base("MyVeryOwnSupplyCrate", "", "")
		{
		}
		public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
		{
			IPrefabRequest task = PrefabDatabase.GetPrefabAsync("580154dd-b2a3-4da1-be14-9a22e20385c8");
			yield return task;
			_ = task.TryGetPrefab(out GameObject prefab);
			GameObject wip = GameObject.Instantiate(prefab);
			wip.SetActive(false);
			gameObject.Set(wip);
			yield break;
		}
	}
}