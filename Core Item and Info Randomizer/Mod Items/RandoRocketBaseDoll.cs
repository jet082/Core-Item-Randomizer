using SMLHelper.V2.Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace CoreItemAndInfoRandomizer
{
	public class RandoRocketBaseDoll : Spawnable
	{
		private static readonly HashSet<string> OuterLadders = new() { "outerLadders1", "outerLadders2", "outerLadders3", "outerLadders4", "outerLadders5", "outerLadders6" };
		private static readonly HashSet<string> InnerLadders = new() { "innerLadder1", "innerLadder2", "innerLadder3", "innerLadder4" };
		private static readonly HashSet<string> SimpleDestroys = new() { "Base_Ladder", "Stage01", "Stage02", "Stage03" };
		public RandoRocketBaseDoll() : base("RandoRocketBaseDoll", "Rocket Platform", "Rocket Platform")
		{
		}
		public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
		{
			TechType baseTechType = TechType.RocketBase;
			IPrefabRequest task = PrefabDatabase.GetPrefabAsync(CraftData.GetClassIdForTechType(baseTechType));
			yield return task;
			_ = task.TryGetPrefab(out GameObject prefab);

			GameObject wip = DollSetup.SetupDoll(prefab, 0.05f);
			foreach (string someLadder in OuterLadders)
			{
				GameObject someLadderObject = wip.transform.Find("Base/Triggers/" + someLadder + "/rocketship_platform_outerLadder").gameObject;
				BoxCollider someLadderObjectCollider = someLadderObject.GetComponent<BoxCollider>();
				GameObject.DestroyImmediate(someLadderObjectCollider);
			}
			foreach (string someLadder in InnerLadders)
			{
				GameObject someLadderObject = wip.transform.Find("Base/Triggers/" + someLadder + "/rocketship_platform_innerLadder").gameObject;
				BoxCollider someLadderObjectCollider = someLadderObject.GetComponent<BoxCollider>();
				GameObject.DestroyImmediate(someLadderObjectCollider);
			}
			foreach (string someSimpleDestroy in SimpleDestroys)
			{
				GameObject.DestroyImmediate(wip.transform.Find(someSimpleDestroy).gameObject);
			}

			WorldForces worldForcesData = wip.EnsureComponent<WorldForces>();
			worldForcesData.aboveWaterGravity = 9.81f;
			worldForcesData.underwaterGravity = 0f;

			gameObject.Set(wip);
			SMLHelper.V2.Handlers.SpriteHandler.RegisterSprite(this.TechType, SpriteManager.Get(baseTechType));
			yield break;
		}
	}
}