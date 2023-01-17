using SMLHelper.V2.Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;
using System.Linq;

namespace CoreItemAndInfoRandomizer
{
	public class RandoCyclopsDoll : Spawnable
	{
		private static readonly Dictionary<string, string> normalnames = new Dictionary<string, string>()
		{
			{ "submarine_launch_bay_01_01_outer_100", "submarine_launch_bay_01_01_normal_265" },
			{ "Cyclops_exterior_submarine_details_18", "Cyclops_exterior_submarine_details_normal_247" },
			{ "submarine_outer_hatch_01_143", "submarine_outer_hatch_01_normal_228" },
			{ "submarine_steering_console_02_145", "submarine_steering_console_02_normal_3659" },
			{ "submarine_wallmods_01_01_146", "submarine_wallmods_01_01_normal_3814" },
			{ "cyclops_submarine_wallmods_01_26", "cyclops_submarine_wallmods_01_normal_3719" },
			{ "Cyclops_exterior_submarine_engine_02_20", "Cyclops_exterior_submarine_engine_02_normal_6938" },
			{ "Cyclops_exterior_submarine_engine_19", "Cyclops_exterior_submarine_engine_normal_7207" },
			{ "cyclops_cabin_23", "cyclops_cabin_normal_204" },
			{ "submarine_control_room_steering_console_base_02_48", "submarine_steering_console_base_02_normal_4313" }
		};
		public RandoCyclopsDoll() : base("RandoCyclopsDoll", "Seamoth", "Seamoth")
		{
		}
		public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
		{
			gameObject.Set(GetGameObject());
			yield break;
		}
		public override GameObject GetGameObject()
		{
			GameObject assetLoaded = SaveAndLoad.Assets.LoadAsset<GameObject>("RandoCyclopsDoll");

			GameObject prefab = GameObject.Instantiate<GameObject>(assetLoaded);
			prefab.name = this.PrefabFileName;

			prefab.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);

			GameObject model = assetLoaded.FindChild("CyclopsDoll");
			model.transform.localPosition = new Vector3(model.transform.localPosition.x, model.transform.localPosition.y + 0.135f, model.transform.localPosition.z);

			var collider = prefab.EnsureComponent<BoxCollider>();
			collider.size = new Vector3(0.06f, 0.148f, 0.42f);
			collider.center = new Vector3(collider.center.x - 0.02f, collider.center.y + 0.135f, collider.center.z - 0.105f);

			var rigidBodyStats = prefab.EnsureComponent<Rigidbody>();
			rigidBodyStats.drag = 1;
			rigidBodyStats.angularDrag = 0.05f;
			rigidBodyStats.mass = 5f;
			rigidBodyStats.isKinematic = true;

			var worldForces = prefab.EnsureComponent<WorldForces>();
			worldForces.handleGravity = true;
			worldForces.aboveWaterGravity = 9.81f;
			worldForces.underwaterGravity = 1f;
			worldForces.handleDrag = true;
			worldForces.aboveWaterDrag = 0.1f;
			worldForces.underwaterDrag = 1f;
			worldForces.moveWithPlatform = false;
			worldForces.enabled = true;

			Shader marmosetUber = Shader.Find("MarmosetUBER");
			var normals = new Dictionary<string, Texture>();
			foreach (KeyValuePair<string, string> elem in normalnames)
				normals.Add(elem.Key, SaveAndLoad.Assets.LoadAsset<Texture>(elem.Value));

			var assetRequest = AddressablesUtility.LoadAsync<GameObject>("Submarine/Build/Aquarium.prefab");
			assetRequest.WaitForCompletion();
			GameObject aquarium = assetRequest.Result;

			Material glass = null;
			Renderer[] aRenderers = aquarium.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer aRenderer in aRenderers)
			{
				foreach (Material aMaterial in aRenderer.materials)
				{
					if (aMaterial.name.StartsWith("Aquarium_glass", StringComparison.OrdinalIgnoreCase))
					{
						glass = aMaterial;
						break;
					}
				}
				if (glass != null)
					break;
			}

			var renderers = prefab.GetAllComponentsInChildren<Renderer>();
			foreach (Renderer rend in renderers)
			{
				if (rend.name.StartsWith("Cyclops_submarine_exterior_glass", true, CultureInfo.InvariantCulture) ||
					rend.name.StartsWith("glass", true, CultureInfo.InvariantCulture))
					rend.material = glass;
				else if (rend.materials.Length > 0)
				{
					foreach (Material tmpMat in rend.materials)
					{
						tmpMat.shader = marmosetUber;
						if (tmpMat.name.StartsWith("cyclops_submarine_exterior_decals_01_24", false, CultureInfo.InvariantCulture))
						{
							tmpMat.SetFloat("_EnableCutOff", 1.0f);
							tmpMat.SetFloat("_Cutoff", 0.1f);
							tmpMat.EnableKeyword("MARMO_ALPHA_CLIP");
						}
						else if (normals != null)
						{
							foreach (KeyValuePair<string, string> elem in normalnames)
							{
								if (tmpMat.name.StartsWith(elem.Key, false, CultureInfo.InvariantCulture))
								{
									if (elem.Value != null && normals.ContainsKey(elem.Value) && normals[elem.Value] != null)
									{
										tmpMat.SetTexture("_BumpMap", normals[elem.Value]);
										tmpMat.EnableKeyword("MARMO_NORMALMAP");
										tmpMat.EnableKeyword("_ZWRITE_ON");
									}
									break;
								}
							}
						}
					}
				}
			}

			BaseModuleLighting bml = prefab.GetComponent<BaseModuleLighting>();
			if (bml == null)
				bml = prefab.GetComponentInChildren<BaseModuleLighting>();
			if (bml == null)
				bml = prefab.AddComponent<BaseModuleLighting>();
			SkyApplier applier = prefab.AddComponent<SkyApplier>();
			if (applier != null)
			{
				applier.renderers = renderers.ToArray(); //renderers;
				applier.anchorSky = Skies.Auto;
				applier.updaterIndex = 0;
			}
			var pickupable = prefab.EnsureComponent<Pickupable>();
			pickupable.isPickupable = true;
			pickupable.randomizeRotationWhenDropped = true;

			SMLHelper.V2.Handlers.SpriteHandler.RegisterSprite(this.TechType, SpriteManager.Get(TechType.Cyclops));
			prefab.EnsureComponent<TechTag>().type = TechType;
			prefab.EnsureComponent<PrefabIdentifier>().ClassId = ClassID;
			return prefab;
		}
	}
}