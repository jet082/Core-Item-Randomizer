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
			PluginSetup.BepinExLogger.LogInfo("lol 1");
			GameObject prefabArmModel = prefab.transform.Find("exosuit_01/root/geoChildren/lArm_clav/ExosuitClawArm(Clone)/exosuit_01_armRight/ArmRig/exosuit_hand_geo").gameObject;
			PluginSetup.BepinExLogger.LogInfo("lol 2");
			GameObject dollArm = Object.Instantiate(prefabArmModel);
			PluginSetup.BepinExLogger.LogInfo("lol 3");

			GameObject wip = DollSetup.SetupDoll(prefab, 1f);
			PluginSetup.BepinExLogger.LogInfo("lol 4");

			dollArm.transform.SetParent(wip.transform, false);
			PluginSetup.BepinExLogger.LogInfo("lol 5");
			dollArm.transform.localRotation = Quaternion.identity;
			dollArm.transform.localPosition = Vector3.zero;
			dollArm.transform.localScale = new Vector3(-1f, 1f, 1f);
			PluginSetup.BepinExLogger.LogInfo("lol 6");

			gameObject.Set(wip);
			yield break;
		}
	}
}
