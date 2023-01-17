using SMLHelper.V2.Assets;
using SMLHelper.V2.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UWE;
using RecipeData = SMLHelper.V2.Crafting.TechData;
using SMLHelper.V2.Crafting;
using System.Globalization;
using System;
using SMLHelper.V2.Utility;

namespace CoreItemAndInfoRandomizer
{
	public class RandoSeamothDoll : Spawnable
	{
		public RandoSeamothDoll() : base("RandoSeamothDoll", "Seamoth", "Seamoth")
		{
		}
		public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
		{
			gameObject.Set(GetGameObject());
			yield break;
		}
		public override GameObject GetGameObject()
		{
			GameObject assetLoaded = SaveAndLoad.Assets.LoadAsset<GameObject>("RandoSeamothDoll");

			GameObject prefab = GameObject.Instantiate<GameObject>(assetLoaded);
			prefab.name = this.PrefabFileName;

			prefab.transform.localScale = new Vector3(5f, 5f, 5f);

			GameObject model = assetLoaded.FindChild("Model");
			model.transform.localPosition = new Vector3(model.transform.localPosition.x, model.transform.localPosition.y + 0.032f, model.transform.localPosition.z);

			var collider = prefab.EnsureComponent<BoxCollider>();
			collider.size = new Vector3(0.07f, 0.054f, 0.07f);
			collider.center = new Vector3(collider.center.x, collider.center.y + 0.027f, collider.center.z);

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

			Shader marmosetUber = Shader.Find("MarmosetUBER");
			Texture normal1 = SaveAndLoad.Assets.LoadAsset<Texture>("power_cell_01_normal");
			Texture spec1 = SaveAndLoad.Assets.LoadAsset<Texture>("power_cell_01_spec");
			Texture illum1 = SaveAndLoad.Assets.LoadAsset<Texture>("power_cell_01_illum");
			Texture normal2 = SaveAndLoad.Assets.LoadAsset<Texture>("Submersible_SeaMoth_normal");
			Texture spec2 = SaveAndLoad.Assets.LoadAsset<Texture>("Submersible_SeaMoth_spec");
			Texture illum2 = SaveAndLoad.Assets.LoadAsset<Texture>("Submersible_SeaMoth_illum");
			Texture normal3 = SaveAndLoad.Assets.LoadAsset<Texture>("Submersible_SeaMoth_indoor_normal");
			Texture illum3 = SaveAndLoad.Assets.LoadAsset<Texture>("Submersible_SeaMoth_indoor_illum");
			Texture normal4 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_storage_01_normal");
			Texture normal5 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_storage_02_normal");
			Texture illum5 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_storage_02_illum");
			Texture normal6 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_power_cell_slot_01_normal");
			Texture spec6 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_power_cell_slot_01_spec");
			Texture normal7 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_torpedo_01_normal");
			Texture spec7 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_torpedo_01_spec");
			Texture normal8 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_torpedo_01_hatch_01_normal");
			Texture spec8 = SaveAndLoad.Assets.LoadAsset<Texture>("seamoth_torpedo_01_hatch_01_spec");

			var renderers = prefab.GetAllComponentsInChildren<Renderer>();
			if (renderers.Length > 0)
			{
				foreach (Renderer rend in renderers)
				{
					if (rend.name.StartsWith("Submersible_SeaMoth_glass_geo", true, CultureInfo.InvariantCulture))
						rend.material = glass;
					else if (rend.materials.Length > 0)
					{
						foreach (Material tmpMat in rend.materials)
						{
							if (string.Compare(tmpMat.name, "Submersible_SeaMoth_Glass_interior (Instance)", true, CultureInfo.InvariantCulture) == 0)
							{
								tmpMat.EnableKeyword("MARMO_SIMPLE_GLASS");
								tmpMat.EnableKeyword("WBOIT");
							}
							else if (string.Compare(tmpMat.name, "Submersible_SeaMoth_Glass (Instance)", true, CultureInfo.InvariantCulture) != 0)
							{
								tmpMat.shader = marmosetUber;
								if (string.Compare(tmpMat.name, "power_cell_01 (Instance)", true, CultureInfo.InvariantCulture) == 0)
								{
									tmpMat.SetTexture("_BumpMap", normal1);
									tmpMat.SetTexture("_SpecTex", spec1);
									tmpMat.SetTexture("_Illum", illum1);
									tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

									tmpMat.EnableKeyword("MARMO_NORMALMAP");
									tmpMat.EnableKeyword("MARMO_EMISSION");
									tmpMat.EnableKeyword("MARMO_SPECMAP");
									tmpMat.EnableKeyword("_ZWRITE_ON"); // Enable Z write
								}
								else if (string.Compare(tmpMat.name, "Submersible_SeaMoth (Instance)", true, CultureInfo.InvariantCulture) == 0)
								{
									tmpMat.SetTexture("_BumpMap", normal2);
									tmpMat.SetTexture("_SpecTex", spec2);
									tmpMat.SetTexture("_Illum", illum2);
									tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

									tmpMat.EnableKeyword("MARMO_NORMALMAP");
									tmpMat.EnableKeyword("MARMO_EMISSION");
									tmpMat.EnableKeyword("MARMO_SPECMAP");
									tmpMat.EnableKeyword("_ZWRITE_ON"); // Enable Z write
								}
								else if (string.Compare(tmpMat.name, "Submersible_SeaMoth_indoor (Instance)", true, CultureInfo.InvariantCulture) == 0)
								{
									tmpMat.SetTexture("_BumpMap", normal3);
									tmpMat.SetTexture("_Illum", illum3);
									tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

									tmpMat.EnableKeyword("MARMO_NORMALMAP");
									tmpMat.EnableKeyword("MARMO_EMISSION");
									tmpMat.EnableKeyword("_ZWRITE_ON"); // Enable Z write
								}
								else if (string.Compare(tmpMat.name, "seamoth_storage_01 (Instance)", true, CultureInfo.InvariantCulture) == 0)
								{
									tmpMat.SetTexture("_BumpMap", normal4);

									tmpMat.EnableKeyword("MARMO_NORMALMAP");
									tmpMat.EnableKeyword("_ZWRITE_ON"); // Enable Z write
								}
								else if (string.Compare(tmpMat.name, "seamoth_storage_02 (Instance)", true, CultureInfo.InvariantCulture) == 0)
								{
									tmpMat.SetTexture("_BumpMap", normal5);
									tmpMat.SetTexture("_Illum", illum5);
									tmpMat.SetFloat("_EmissionLM", 0.75f); // Set always visible

									tmpMat.EnableKeyword("MARMO_NORMALMAP");
									tmpMat.EnableKeyword("MARMO_EMISSION");
									tmpMat.EnableKeyword("_ZWRITE_ON"); // Enable Z write
								}
								else if (string.Compare(tmpMat.name, "seamoth_power_cell_slot_01 (Instance)", true, CultureInfo.InvariantCulture) == 0)
								{
									tmpMat.SetTexture("_BumpMap", normal6);
									tmpMat.SetTexture("_SpecTex", spec6);

									tmpMat.EnableKeyword("MARMO_NORMALMAP");
									tmpMat.EnableKeyword("MARMO_SPECMAP");
									tmpMat.EnableKeyword("_ZWRITE_ON"); // Enable Z write
								}
								else if (string.Compare(tmpMat.name, "seamoth_torpedo_01 (Instance)", true, CultureInfo.InvariantCulture) == 0)
								{
									tmpMat.SetTexture("_BumpMap", normal7);
									tmpMat.SetTexture("_SpecTex", spec7);

									tmpMat.EnableKeyword("MARMO_NORMALMAP");
									tmpMat.EnableKeyword("MARMO_SPECMAP");
									tmpMat.EnableKeyword("_ZWRITE_ON"); // Enable Z write
								}
								else if (string.Compare(tmpMat.name, "seamoth_torpedo_01_hatch_01 (Instance)", true, CultureInfo.InvariantCulture) == 0)
								{
									tmpMat.SetTexture("_BumpMap", normal8);
									tmpMat.SetTexture("_SpecTex", spec8);

									tmpMat.EnableKeyword("MARMO_NORMALMAP");
									tmpMat.EnableKeyword("MARMO_SPECMAP");
									tmpMat.EnableKeyword("_ZWRITE_ON"); // Enable Z write
								}
							}
						}
					}
				}
			}
			var pickupable = prefab.EnsureComponent<Pickupable>();
			pickupable.isPickupable = true;
			pickupable.randomizeRotationWhenDropped = true;

			SMLHelper.V2.Handlers.SpriteHandler.RegisterSprite(this.TechType, SpriteManager.Get(TechType.Seamoth));
			prefab.EnsureComponent<TechTag>().type = TechType;
			prefab.EnsureComponent<PrefabIdentifier>().ClassId = ClassID;
			return prefab;
		}
	}
}
