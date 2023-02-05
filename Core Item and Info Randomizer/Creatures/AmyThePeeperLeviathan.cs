using SMLHelper.V2.Assets;
using System.Collections;
using UnityEngine;
using UWE;

namespace CoreItemAndInfoRandomizer
{
	public class AmyThePeeperLeviathan : Spawnable
	{
		public AmyThePeeperLeviathan() : base("AmyThePeeperLeviathan", "Amy The Peeper Leviathan", "It's Amy!")
		{
		}
		public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
		{
			TechType baseTechType = TechType.Peeper;
			IPrefabRequest task = PrefabDatabase.GetPrefabAsync(CraftData.GetClassIdForTechType(baseTechType));
			yield return task;
			_ = task.TryGetPrefab(out GameObject prefab);
			GameObject wip = GameObject.Instantiate(prefab);
			wip.GetComponent<Peeper>().BecomeHero();
			wip.EnsureComponent<SpecialResizable>().ScaleFactor = 40f;

			GameObject.DestroyImmediate(wip.GetComponent<Pickupable>());
			gameObject.Set(wip);
			SMLHelper.V2.Handlers.SpriteHandler.RegisterSprite(this.TechType, SpriteManager.Get(TechType.Peeper));
			yield break;
		}
	}
}