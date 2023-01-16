using SMLHelper.V2.Assets;
using SMLHelper.V2.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UWE;
using RecipeData = SMLHelper.V2.Crafting.TechData;
using SMLHelper.V2.Crafting;
using DecorationsMod;
using DecorationsMod.Controllers;

namespace CoreItemAndInfoRandomizer
{
	public class RandoSeamothDoll : Craftable
	{
		public RandoSeamothDoll() : base("RandoSeamothDoll", "Seamoth", "Seamoth")
		{
		}
		public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
		{
			_ = TechTypeHandler.TryGetModdedTechType("SeamothDoll", out TechType outTechType);
			IPrefabRequest task = PrefabDatabase.GetPrefabAsync(CraftData.GetClassIdForTechType(outTechType));
			yield return task;
			_ = task.TryGetPrefab(out GameObject prefab);
			GameObject dollObj = Object.Instantiate(prefab);
			
			var model = dollObj.FindChild("Model");
			GameObject extras = model.FindChild("Submersible_SeaMoth_extras");
			Renderer[] extraRenderers = extras.GetAllComponentsInChildren<Renderer>();
			foreach (Renderer renderer in extraRenderers)
				renderer.enabled = false;

			Component.Destroy(dollObj.GetComponent<SeamothDollController>());

			PlayerTool playerTool = dollObj.EnsureComponent<PlayerTool>();
			playerTool.mainCollider = dollObj.EnsureComponent<BoxCollider>();

			PlaceTool placeTool = dollObj.EnsureComponent<GenericPlaceTool>();
			placeTool.allowedInBase = true;
			placeTool.allowedOnBase = true;
			placeTool.allowedOnCeiling = true;
			placeTool.allowedOnConstructable = true;
			placeTool.allowedOnGround = true;
			placeTool.allowedOnRigidBody = true;
			placeTool.allowedOnWalls = true;
			placeTool.allowedOutside = true;
			placeTool.allowedUnderwater = true;
			placeTool.reloadMode = PlayerTool.ReloadMode.None;
			placeTool.socket = PlayerTool.Socket.RightHand;
			placeTool.rotationEnabled = true;
			placeTool.hasAnimations = false;
			placeTool.hasBashAnimation = false;
			placeTool.hasFirstUseAnimation = false;

			HandTarget handTarget = dollObj.EnsureComponent<HandTarget>();
			handTarget._isvalid = true;
			handTarget.creationTime = 3f;


			Pickupable pickupable = dollObj.EnsureComponent<Pickupable>();
			pickupable.attached = false;
			pickupable.isPickupable = true;
			pickupable.isValidHandTarget = true;
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

			gameObject.Set(dollObj);
		}
		protected override RecipeData GetBlueprintRecipe()
		{
			return new RecipeData() { craftAmount = 1, Ingredients = new List<Ingredient>(), LinkedItems = new List<TechType>() };
		}
	}
}
