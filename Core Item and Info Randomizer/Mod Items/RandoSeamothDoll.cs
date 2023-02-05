using SMLHelper.V2.Assets;
using System.Collections;
using UnityEngine;
using UWE;

namespace CoreItemAndInfoRandomizer
{
	public class RandoSeamothDoll : Spawnable
	{
		public RandoSeamothDoll() : base("RandoSeamothDoll", "Seamoth", "Seamoth")
		{
		}
		public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
		{
			TechType baseTechType = TechType.Seamoth;
			IPrefabRequest task = PrefabDatabase.GetPrefabAsync(CraftData.GetClassIdForTechType(baseTechType));
			yield return task;
			_ = task.TryGetPrefab(out GameObject prefab);
			GameObject wip = DollSetup.SetupDoll(prefab, 0.2f);
			GameObject.DestroyImmediate(wip.transform.Find("SeamothDamageFXSpawn").gameObject);
			gameObject.Set(wip);
			SMLHelper.V2.Handlers.SpriteHandler.RegisterSprite(this.TechType, SpriteManager.Get(baseTechType));
			yield break;
		}
	}
}