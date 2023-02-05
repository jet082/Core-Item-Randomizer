using SMLHelper.V2.Assets;
using System.Collections;
using UnityEngine;
using UWE;

namespace CoreItemAndInfoRandomizer
{
	public class RandoRocketBaseLadderDoll : Spawnable
	{
		public RandoRocketBaseLadderDoll() : base("RandoRocketBaseLadderDoll", "Neptune Gantry", "Neptune Gantry")
		{
		}
		public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
		{
			TechType baseTechType = TechType.RocketBase;
			IPrefabRequest task = PrefabDatabase.GetPrefabAsync(CraftData.GetClassIdForTechType(baseTechType));
			yield return task;
			_ = task.TryGetPrefab(out GameObject prefab);
			GameObject ladderObject = prefab.transform.Find("Base_Ladder").gameObject;
			float scaler = 0.05f;
			GameObject wip = DollSetup.SetupDoll(ladderObject, scaler);
			PrefabIdentifier prefabIdentifier = wip.EnsureComponent<PrefabIdentifier>();
			prefabIdentifier.ClassId = "RandoRocketBaseLadderDoll";
			WorldForces worldForcesData = wip.EnsureComponent<WorldForces>();
			worldForcesData.aboveWaterGravity = 9.81f;
			worldForcesData.underwaterGravity = 0f;
			worldForcesData.handleDrag = true;
			worldForcesData.underwaterDrag = 1f;
			worldForcesData.enabled = true;
			Rigidbody rigidBodyData = wip.EnsureComponent<Rigidbody>();
			rigidBodyData.drag = 1f;
			rigidBodyData.angularDrag = 4f;
			rigidBodyData.mass = 800f;
			rigidBodyData.useGravity = true;
			rigidBodyData.isKinematic = true;
			GameObject.DestroyImmediate(wip.transform.Find("ElevatorControls_Top/Collision").gameObject);
			GameObject.DestroyImmediate(wip.transform.Find("ElevatorControls_Bottom/Collision").gameObject);


			gameObject.Set(wip);
			SMLHelper.V2.Handlers.SpriteHandler.RegisterSprite(this.TechType, SpriteManager.Get(TechType.RocketBase));
			yield break;
		}
	}
}