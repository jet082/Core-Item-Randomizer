using SMLHelper.V2.Assets;
using System.Collections;
using UnityEngine;
using UWE;

namespace CoreItemAndInfoRandomizer
{
	public class RandoPrawnSuitDoll : Spawnable
	{
		public RandoPrawnSuitDoll() : base("RandoPrawnSuitDoll", "Prawn Suit", "Prawn Suit")
		{
		}
		public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
		{
			TechType baseTechType = TechType.Exosuit;
			IPrefabRequest task = PrefabDatabase.GetPrefabAsync(CraftData.GetClassIdForTechType(baseTechType));
			yield return task;
			_ = task.TryGetPrefab(out GameObject prefab);

			GameObject wip = DollSetup.SetupDoll(prefab, 0.1f);

			TechType baseTechType2 = TechType.ExosuitClawArmModule;

			IPrefabRequest task2 = PrefabDatabase.GetPrefabAsync(CraftData.GetClassIdForTechType(baseTechType2));
			yield return task2;
			_ = task2.TryGetPrefab(out GameObject prefab2);
			PluginSetup.BepinExLogger.LogInfo($"Now at {wip.transform.childCount}");
			GameObject arm1g = Object.Instantiate<GameObject>(prefab2);
			arm1g.transform.parent = wip.transform;
			arm1g.transform.localRotation = Quaternion.identity;
			arm1g.transform.localPosition = Vector3.zero;
			arm1g.transform.localScale = new Vector3(-1f, 1f, 1f);
			arm1g.transform.SetParent(wip.transform, false);

			PluginSetup.BepinExLogger.LogInfo($"Now at {wip.transform.childCount}");

			gameObject.Set(wip);
			yield break;
		}
	}
}
