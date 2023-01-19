using SMLHelper.V2.Assets;
using System.Collections;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class RandoCyclopsDoll : Spawnable
	{
		private Vector3 SpawnPosition;
		private Quaternion SpawnRotation;
		private GameObject LastCreatedSub;
		public RandoCyclopsDoll() : base("RandoCyclopsDoll", "Cyclops", "Cyclops")
		{
		}
		public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
		{
			Transform transform = MainCamera.camera.transform;
			SpawnPosition = transform.position + 20f * transform.forward;
			SpawnRotation = Quaternion.LookRotation(MainCamera.camera.transform.right);
			LightmappedPrefabs.main.RequestScenePrefab("cyclops", new LightmappedPrefabs.OnPrefabLoaded(OnSubPrefabLoaded));
			if (LastCreatedSub != null)
			{
				gameObject.Set(GetGameObject());
			}
			yield break;
		}
		private void OnSubPrefabLoaded(GameObject prefab)
		{
			GameObject gameObject = Utils.SpawnPrefabAt(prefab, null, this.SpawnPosition);
			gameObject.transform.rotation = this.SpawnRotation;
			gameObject.SetActive(false);
			gameObject.SendMessage("StartConstruction", SendMessageOptions.DontRequireReceiver);
			LargeWorldEntity.Register(gameObject);
			CrafterLogic.NotifyCraftEnd(gameObject, CraftData.GetTechType(gameObject));
			LastCreatedSub = gameObject;
		}
	}
}