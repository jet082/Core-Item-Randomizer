using SMLHelper.V2.Assets;
using System.Collections;
using UnityEngine;
using System.Globalization;
using System;

namespace CoreItemAndInfoRandomizer
{
	public class RandoPrawnSuitDoll : Spawnable
	{
		public RandoPrawnSuitDoll() : base("RandoPrawnSuitDoll", "Prawn Suit", "Prawn Suit")
		{
		}
		public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
		{
			gameObject.Set(GetGameObject());
			yield break;
		}
		public override GameObject GetGameObject()
		{
			GameObject assetLoaded = SaveAndLoad.Assets.LoadAsset<GameObject>("RandoPrawnSuitDoll");
			GameObject prefab = GameObject.Instantiate<GameObject>(assetLoaded);
			prefab.name = this.PrefabFileName;

			prefab.transform.localScale = new Vector3(4f, 4f, 4f);
			GameObject model = assetLoaded.FindChild("prawnsuit");
			model.transform.localPosition = new Vector3(model.transform.localPosition.x, model.transform.localPosition.y - 0.002f, model.transform.localPosition.z);

			var collider = prefab.EnsureComponent<BoxCollider>();
			collider.size = new Vector3(0.04f, 0.115f, 0.04f);
			collider.center = new Vector3(collider.center.x, collider.center.y + 0.0575f, collider.center.z);

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
			Shader shader = Shader.Find("MarmosetUBER");
			Texture normal = SaveAndLoad.Assets.LoadAsset<Texture>("exosuit_01_normal");
			Texture spec = SaveAndLoad.Assets.LoadAsset<Texture>("exosuit_01_spec");
			Texture colorMask = SaveAndLoad.Assets.LoadAsset<Texture>("exosuit_01_colorMask");
			Texture illum = SaveAndLoad.Assets.LoadAsset<Texture>("exosuit_01_illum");
			Texture normal2 = SaveAndLoad.Assets.LoadAsset<Texture>("exosuit_01_fp_normal");
			Texture illum2 = SaveAndLoad.Assets.LoadAsset<Texture>("exosuit_01_fp_illum");
			Texture normal3 = SaveAndLoad.Assets.LoadAsset<Texture>("Exosuit_Arm_Propulsion_Cannon_normal");
			Texture colorMask3 = SaveAndLoad.Assets.LoadAsset<Texture>("Exosuit_Arm_Propulsion_Cannon_colorMask");
			Texture illum3 = SaveAndLoad.Assets.LoadAsset<Texture>("Exosuit_Arm_Propulsion_Cannon_illum");
			Texture normal4 = SaveAndLoad.Assets.LoadAsset<Texture>("Exosuit_grappling_arm_normal");
			Texture colorMask4 = SaveAndLoad.Assets.LoadAsset<Texture>("Exosuit_grappling_arm_colorMask");
			Texture illum4 = SaveAndLoad.Assets.LoadAsset<Texture>("Exosuit_grappling_arm_illum");
			Texture normal5 = SaveAndLoad.Assets.LoadAsset<Texture>("exosuit_01_glass_normal");
			Texture normal6 = SaveAndLoad.Assets.LoadAsset<Texture>("exosuit_storage_01_normal");
			Texture colorMask6 = SaveAndLoad.Assets.LoadAsset<Texture>("exosuit_storage_01_colorMask");
			Texture illum6 = SaveAndLoad.Assets.LoadAsset<Texture>("exosuit_storage_01_illum");
			Texture normal7 = SaveAndLoad.Assets.LoadAsset<Texture>("Exosuit_torpedo_launcher_arm_normal");
			Texture colorMask7 = SaveAndLoad.Assets.LoadAsset<Texture>("Exosuit_torpedo_launcher_arm_colorMask");
			Texture illum7 = SaveAndLoad.Assets.LoadAsset<Texture>("Exosuit_torpedo_launcher_arm_illum");
			Texture normal8 = SaveAndLoad.Assets.LoadAsset<Texture>("engine_power_cell_ion_normal");
			Texture spec8 = SaveAndLoad.Assets.LoadAsset<Texture>("engine_power_cell_ion_spec");
			Texture illum8 = SaveAndLoad.Assets.LoadAsset<Texture>("engine_power_cell_ion_illum");
			Texture normal9 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_torpedo_01_normal");
			Texture spec9 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_torpedo_01_spec");
			Texture normal10 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_upgrade_slots_01_normal");
			Texture spec10 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_upgrade_slots_01_spec");
			Texture illum10 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_upgrade_slots_01_illum");
			Texture normal11 = SaveAndLoad.Assets.LoadAsset<Texture>("submarine_engine_power_cells_01_normal");
			Texture spec11 = SaveAndLoad.Assets.LoadAsset<Texture>("submarine_engine_power_cells_01_spec");
			Texture illum11 = SaveAndLoad.Assets.LoadAsset<Texture>("submarine_engine_power_cells_01_illum");

			Renderer[] renderers = prefab.GetAllComponentsInChildren<Renderer>();
			foreach (Renderer renderer in renderers)
			{
				if (renderer.name.StartsWith("Exosuit_cabin_01_glass", true, CultureInfo.InvariantCulture))
					renderer.material = glass;
				else if (renderer.materials != null)
				{
					foreach (Material tmpMat in renderer.materials)
					{
						// Associate MarmosetUBER shader
						if (string.Compare(tmpMat.name, "exosuit_cabin_01_glass (Instance)", true, CultureInfo.InvariantCulture) == 0)
						{
							tmpMat.EnableKeyword("MARMO_SIMPLE_GLASS");
							tmpMat.EnableKeyword("WBOIT");
						}
						else if (string.Compare(tmpMat.name, "exosuit_01_glass (Instance)", true, CultureInfo.InvariantCulture) != 0)
							tmpMat.shader = shader;

						if (string.Compare(tmpMat.name, "exosuit_01 (Instance)", true, CultureInfo.InvariantCulture) == 0)
						{
							tmpMat.SetTexture("_BumpMap", normal);
							tmpMat.SetTexture("_ColorMask", colorMask);
							tmpMat.SetTexture("_SpecTex", spec);
							tmpMat.SetTexture("_Illum", illum);
							tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

							// Enable color mask
							tmpMat.EnableKeyword("MARMO_VERTEX_COLOR");
							// Enable specular
							//tmpMat.EnableKeyword("MARMO_SPECULAR_ON");
							tmpMat.EnableKeyword("MARMO_SPECULAR_IBL");
							tmpMat.EnableKeyword("MARMO_SPECULAR_DIRECT");
							tmpMat.EnableKeyword("MARMO_SPECMAP");
							tmpMat.EnableKeyword("MARMO_MIP_GLOSS");
							// Enable normal map
							tmpMat.EnableKeyword("MARMO_NORMALMAP");
							// Enable emission map
							tmpMat.EnableKeyword("MARMO_EMISSION");
							// Enable Z write
							tmpMat.EnableKeyword("_ZWRITE_ON");
						}
						else if (string.Compare(tmpMat.name, "exosuit_01_fp (Instance)", true, CultureInfo.InvariantCulture) == 0)
						{
							tmpMat.SetTexture("_BumpMap", normal2);
							tmpMat.SetTexture("_Illum", illum2);
							tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

							// Enable normal map
							tmpMat.EnableKeyword("MARMO_NORMALMAP");
							// Enable emission map
							tmpMat.EnableKeyword("MARMO_EMISSION");
							// Enable Z write
							tmpMat.EnableKeyword("_ZWRITE_ON");
						}
						else if (string.Compare(tmpMat.name, "Exosuit_Arm_Propulsion_Cannon (Instance)", true, CultureInfo.InvariantCulture) == 0)
						{
							tmpMat.SetTexture("_BumpMap", normal3);
							tmpMat.SetTexture("_ColorMask", colorMask3);
							tmpMat.SetTexture("_Illum", illum3);
							tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

							// Enable color mask
							tmpMat.EnableKeyword("MARMO_VERTEX_COLOR");
							// Enable normal map
							tmpMat.EnableKeyword("MARMO_NORMALMAP");
							// Enable emission map
							tmpMat.EnableKeyword("MARMO_EMISSION");
							// Enable Z write
							tmpMat.EnableKeyword("_ZWRITE_ON");
						}
						else if (string.Compare(tmpMat.name, "Exosuit_grappling_arm (Instance)", true, CultureInfo.InvariantCulture) == 0)
						{
							tmpMat.SetTexture("_BumpMap", normal4);
							tmpMat.SetTexture("_ColorMask", colorMask4);
							tmpMat.SetTexture("_Illum", illum4);
							tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

							// Enable color mask
							tmpMat.EnableKeyword("MARMO_VERTEX_COLOR");
							// Enable normal map
							tmpMat.EnableKeyword("MARMO_NORMALMAP");
							// Enable emission map
							tmpMat.EnableKeyword("MARMO_EMISSION");
							// Enable Z write
							tmpMat.EnableKeyword("_ZWRITE_ON");
						}
						/*
						else if (tmpMat.name.CompareTo("exosuit_01_glass (Instance)") == 0)
						{
							tmpMat.SetTexture("_BumpMap", normal5);
							tmpMat.EnableKeyword("MARMO_SIMPLE_GLASS");
							tmpMat.EnableKeyword("MARMO_NORMALMAP");
							tmpMat.EnableKeyword("WBOIT");
							tmpMat.EnableKeyword("_ZWRITE_ON");
						}
						else if (tmpMat.name.CompareTo("exosuit_cabin_01_glass (Instance)") == 0)
						{
							tmpMat.EnableKeyword("MARMO_SIMPLE_GLASS");
							tmpMat.EnableKeyword("WBOIT");
						}
						*/
						else if (string.Compare(tmpMat.name, "exosuit_storage_01 (Instance)", true, CultureInfo.InvariantCulture) == 0)
						{
							tmpMat.SetTexture("_BumpMap", normal6);
							tmpMat.SetTexture("_ColorMask", colorMask6);
							tmpMat.SetTexture("_Illum", illum6);
							tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

							// Enable color mask
							tmpMat.EnableKeyword("MARMO_VERTEX_COLOR");
							// Enable normal map
							tmpMat.EnableKeyword("MARMO_NORMALMAP");
							// Enable emission map
							tmpMat.EnableKeyword("MARMO_EMISSION");
							// Enable Z write
							tmpMat.EnableKeyword("_ZWRITE_ON");
						}
						else if (string.Compare(tmpMat.name, "Exosuit_torpedo_launcher_arm (Instance)", true, CultureInfo.InvariantCulture) == 0)
						{
							tmpMat.SetTexture("_BumpMap", normal7);
							tmpMat.SetTexture("_ColorMask", colorMask7);
							tmpMat.SetTexture("_Illum", illum7);
							tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

							// Enable color mask
							tmpMat.EnableKeyword("MARMO_VERTEX_COLOR");
							// Enable normal map
							tmpMat.EnableKeyword("MARMO_NORMALMAP");
							// Enable emission map
							tmpMat.EnableKeyword("MARMO_EMISSION");
							// Enable Z write
							tmpMat.EnableKeyword("_ZWRITE_ON");
						}
						else if (string.Compare(tmpMat.name, "power_cell_01 (Instance)", true, CultureInfo.InvariantCulture) == 0)
						{
							tmpMat.SetTexture("_BumpMap", normal8);
							tmpMat.SetTexture("_SpecTex", spec8);
							tmpMat.SetTexture("_Illum", illum8);
							tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

							// Enable specular
							tmpMat.EnableKeyword("MARMO_SPECULAR_IBL");
							tmpMat.EnableKeyword("MARMO_SPECULAR_DIRECT");
							tmpMat.EnableKeyword("MARMO_SPECMAP");
							tmpMat.EnableKeyword("MARMO_MIP_GLOSS");
							// Enable normal map
							tmpMat.EnableKeyword("MARMO_NORMALMAP");
							// Enable emission map
							tmpMat.EnableKeyword("MARMO_EMISSION");
							// Enable Z write
							tmpMat.EnableKeyword("_ZWRITE_ON");
						}
						else if (string.Compare(tmpMat.name, "seamoth_torpedo_01 (Instance)", true, CultureInfo.InvariantCulture) == 0)
						{
							tmpMat.SetTexture("_BumpMap", normal9);
							tmpMat.SetTexture("_SpecTex", spec9);
							tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

							// Enable specular
							tmpMat.EnableKeyword("MARMO_SPECULAR_IBL");
							tmpMat.EnableKeyword("MARMO_SPECULAR_DIRECT");
							tmpMat.EnableKeyword("MARMO_SPECMAP");
							tmpMat.EnableKeyword("MARMO_MIP_GLOSS");
							// Enable normal map
							tmpMat.EnableKeyword("MARMO_NORMALMAP");
							// Enable Z write
							tmpMat.EnableKeyword("_ZWRITE_ON");
						}
						else if (string.Compare(tmpMat.name, "seamoth_upgrade_slots_01 (Instance)", true, CultureInfo.InvariantCulture) == 0)
						{
							tmpMat.SetTexture("_BumpMap", normal10);
							tmpMat.SetTexture("_SpecTex", spec10);
							tmpMat.SetTexture("_Illum", illum10);
							tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

							// Enable specular
							tmpMat.EnableKeyword("MARMO_SPECULAR_IBL");
							tmpMat.EnableKeyword("MARMO_SPECULAR_DIRECT");
							tmpMat.EnableKeyword("MARMO_SPECMAP");
							tmpMat.EnableKeyword("MARMO_MIP_GLOSS");
							// Enable normal map
							tmpMat.EnableKeyword("MARMO_NORMALMAP");
							// Enable emission map
							tmpMat.EnableKeyword("MARMO_EMISSION");
							// Enable Z write
							tmpMat.EnableKeyword("_ZWRITE_ON");
						}
						else if (string.Compare(tmpMat.name, "submarine_engine_power_cells_01 (Instance)", true, CultureInfo.InvariantCulture) == 0)
						{
							tmpMat.SetTexture("_BumpMap", normal11);
							tmpMat.SetTexture("_SpecTex", spec11);
							tmpMat.SetTexture("_Illum", illum11);
							tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

							// Enable specular
							tmpMat.EnableKeyword("MARMO_SPECULAR_IBL");
							tmpMat.EnableKeyword("MARMO_SPECULAR_DIRECT");
							tmpMat.EnableKeyword("MARMO_SPECMAP");
							tmpMat.EnableKeyword("MARMO_MIP_GLOSS");
							// Enable normal map
							tmpMat.EnableKeyword("MARMO_NORMALMAP");
							// Enable emission map
							tmpMat.EnableKeyword("MARMO_EMISSION");
							// Enable Z write
							tmpMat.EnableKeyword("_ZWRITE_ON");
						}
					}
				}
			}
			// Add sky applier
			BaseModuleLighting bml = prefab.GetComponent<BaseModuleLighting>();
			if (bml == null)
				bml = prefab.GetComponentInChildren<BaseModuleLighting>();
			if (bml == null)
				bml = prefab.AddComponent<BaseModuleLighting>();
			SkyApplier applier = prefab.GetComponent<SkyApplier>();
			if (applier == null)
				applier = prefab.GetComponentInChildren<SkyApplier>();
			if (applier == null)
				applier = prefab.AddComponent<SkyApplier>();
			applier.renderers = renderers;
			applier.anchorSky = Skies.Auto;
			applier.updaterIndex = 0;
			SkyApplier[] appliers = prefab.GetComponentsInChildren<SkyApplier>();
			if (appliers != null && appliers.Length > 0)
			{
				foreach (SkyApplier ap in appliers)
				{
					ap.renderers = renderers;
					ap.anchorSky = Skies.Auto;
					ap.updaterIndex = 0;
				}
			}
			
			var pickupable = prefab.EnsureComponent<Pickupable>();
			pickupable.isPickupable = true;
			pickupable.randomizeRotationWhenDropped = true;

			SMLHelper.V2.Handlers.SpriteHandler.RegisterSprite(this.TechType, SpriteManager.Get(TechType.Exosuit));
			prefab.EnsureComponent<TechTag>().type = TechType;
			prefab.EnsureComponent<PrefabIdentifier>().ClassId = ClassID;
			return prefab;
		}
	}
}
