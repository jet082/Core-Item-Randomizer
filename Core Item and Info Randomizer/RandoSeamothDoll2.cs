using System;
using System.Globalization;
using UnityEngine;
using UWE;
using DecorationsMod;
using DecorationsMod.Controllers;

namespace CoreItemAndInfoRandomizer
{
	public class RandoSeamothDoll2 : SMLHelper.V2.Assets.ModPrefab
	{
		private static GameObject _RandoSeamothDoll2 = null;

		public override GameObject GetGameObject()
		{
			GameObject prefab = GameObject.Instantiate(_RandoSeamothDoll2);

			prefab.name = this.ClassID;
			prefab.transform.localScale *= 5.0f;

			return prefab;
		}
		public RandoSeamothDoll2() : base("", "")
		{
			ClassID = "RandoSeamothDoll2";
			TechType = SMLHelper.V2.Handlers.TechTypeHandler.AddTechType(this.ClassID, "", "", true);
			PrefabFileName = "WorldEntities/Environment/Wrecks/RandoSeamothDoll2";
		}
		public void RegisterItem()
		{
			if (_RandoSeamothDoll2 == null)
				_RandoSeamothDoll2 = AssetsHelper.Assets.LoadAsset<GameObject>("seamothpuppet");
			GameObject model = _RandoSeamothDoll2.FindChild("Model");
			model.transform.localPosition = new Vector3(model.transform.localPosition.x, model.transform.localPosition.y + 0.032f, model.transform.localPosition.z);

			var prefabId = _RandoSeamothDoll2.EnsureComponent<PrefabIdentifier>();
			prefabId.ClassId = ClassID;

			LargeWorldEntity lwe = _RandoSeamothDoll2.GetComponent<LargeWorldEntity>();
			if (lwe == null)
				lwe = _RandoSeamothDoll2.EnsureComponent<LargeWorldEntity>();
			lwe.cellLevel = LargeWorldEntity.CellLevel.Global;
			lwe.updaterIndex = 0;
			lwe.hideFlags = HideFlags.None;
			lwe.useGUILayout = true;
			lwe.enabled = true;

			WorldEntityData wed = _RandoSeamothDoll2.GetComponent<WorldEntityData>();

			var techTag = _RandoSeamothDoll2.EnsureComponent<TechTag>();
			techTag.type = this.TechType;

			var collider = _RandoSeamothDoll2.EnsureComponent<BoxCollider>();
			collider.size = new Vector3(0.07f, 0.054f, 0.07f);
			collider.center = new Vector3(collider.center.x, collider.center.y + 0.027f, collider.center.z);
			var rigidBodyStats = _RandoSeamothDoll2.EnsureComponent<Rigidbody>();
			rigidBodyStats.drag = 1;
			rigidBodyStats.angularDrag = 0.05f;
			rigidBodyStats.mass = 5f;
			rigidBodyStats.isKinematic = true;

			var worldForces = _RandoSeamothDoll2.EnsureComponent<WorldForces>();
			worldForces.handleGravity = true;
			worldForces.aboveWaterGravity = 9.81f;
			worldForces.underwaterGravity = 1f;
			worldForces.handleDrag = true;
			worldForces.aboveWaterDrag = 0.1f;
			worldForces.underwaterDrag = 1f;
			worldForces.moveWithPlatform = false;

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
			Texture normal1 = AssetsHelper.Assets.LoadAsset<Texture>("power_cell_01_normal");
			Texture spec1 = AssetsHelper.Assets.LoadAsset<Texture>("power_cell_01_spec");
			Texture illum1 = AssetsHelper.Assets.LoadAsset<Texture>("power_cell_01_illum");
			Texture normal2 = AssetsHelper.Assets.LoadAsset<Texture>("Submersible_SeaMoth_normal");
			Texture spec2 = AssetsHelper.Assets.LoadAsset<Texture>("Submersible_SeaMoth_spec");
			Texture illum2 = AssetsHelper.Assets.LoadAsset<Texture>("Submersible_SeaMoth_illum");
			Texture normal3 = AssetsHelper.Assets.LoadAsset<Texture>("Submersible_SeaMoth_indoor_normal");
			Texture illum3 = AssetsHelper.Assets.LoadAsset<Texture>("Submersible_SeaMoth_indoor_illum");
			Texture normal4 = AssetsHelper.Assets.LoadAsset<Texture>("seamoth_storage_01_normal");
			Texture normal5 = AssetsHelper.Assets.LoadAsset<Texture>("seamoth_storage_02_normal");
			Texture illum5 = AssetsHelper.Assets.LoadAsset<Texture>("seamoth_storage_02_illum");
			Texture normal6 = AssetsHelper.Assets.LoadAsset<Texture>("seamoth_power_cell_slot_01_normal");
			Texture spec6 = AssetsHelper.Assets.LoadAsset<Texture>("seamoth_power_cell_slot_01_spec");
			Texture normal7 = AssetsHelper.Assets.LoadAsset<Texture>("seamoth_torpedo_01_normal");
			Texture spec7 = AssetsHelper.Assets.LoadAsset<Texture>("seamoth_torpedo_01_spec");
			Texture normal8 = AssetsHelper.Assets.LoadAsset<Texture>("seamoth_torpedo_01_hatch_01_normal");
			Texture spec8 = AssetsHelper.Assets.LoadAsset<Texture>("seamoth_torpedo_01_hatch_01_spec");

			var renderers = _RandoSeamothDoll2.GetAllComponentsInChildren<Renderer>();
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

			GameObject extras = model.FindChild("Submersible_SeaMoth_extras");
			GameObject extra = extras.FindChild("Submersible_seaMoth_geo 1").FindChild("seaMoth_storage_01_L_geo");
			Renderer[] extraRenderers = extras.GetAllComponentsInChildren<Renderer>();
			foreach (Renderer renderer in extraRenderers)
				renderer.enabled = false;

			Pickupable pickupable = _RandoSeamothDoll2.EnsureComponent<Pickupable>();
			pickupable.attached = false;
			pickupable.isPickupable = true;
			pickupable.isValidHandTarget = true;
			pickupable.overrideTechType = TechType.None;
			pickupable.overrideTechUsed = false;
			pickupable.isLootCube = false;
			pickupable.destroyOnDeath = false;
			pickupable.version = 0;
			pickupable.isKinematic = PickupableKinematicState.NoKinematicStateSet;
			pickupable.randomizeRotationWhenDropped = false;
			pickupable.activateRigidbodyWhenDropped = true;
			pickupable.usePackUpIcon = false;
			pickupable.hideFlags = HideFlags.None;
			pickupable.useGUILayout = true;
			pickupable.enabled = true;
			Component.Destroy(_RandoSeamothDoll2.GetComponent<SeamothDollController>());
			SMLHelper.V2.Handlers.PrefabHandler.RegisterPrefab(this);
			SMLHelper.V2.Handlers.SpriteHandler.RegisterSprite(TechType, SpriteManager.Get(TechType.Seamoth));
		}
	}
}