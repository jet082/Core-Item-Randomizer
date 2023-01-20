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
			Exosuit tempExosuitData = prefab.GetComponent<Exosuit>();
			GameObject clawArmPrefab = tempExosuitData.armPrefabs[0].prefab.gameObject;
			GameObject leftClawArm = GameObject.Instantiate(clawArmPrefab);
			GameObject rightClawArm = GameObject.Instantiate(clawArmPrefab);

			float scaler = 0.12f;
			GameObject wip = DollSetup.SetupDoll(prefab, scaler);
			GameObject.DestroyImmediate(wip.transform.Find("BatterySlot2").gameObject.GetComponent<EnergyMixin>());
			GameObject.DestroyImmediate(wip.transform.Find("exosuit_01/root/geoChildren/upgrade_geoHldr/Storage").gameObject.GetComponent<StorageContainer>());
			leftClawArm.transform.parent = wip.transform;
			leftClawArm.transform.localRotation = Quaternion.identity;
			leftClawArm.transform.localPosition = new Vector3(4.33f * scaler, 1f, 0f);
			leftClawArm.transform.localScale = new Vector3(-1f, 1f, 1f);

			rightClawArm.transform.parent = wip.transform;
			rightClawArm.transform.localRotation = Quaternion.identity;
			rightClawArm.transform.localPosition = new Vector3(-4.33f * scaler, 1f, 0f);
			rightClawArm.transform.localScale = new Vector3(1f, 1f, 1f);

			gameObject.Set(wip);
			yield break;
		}
	}
}