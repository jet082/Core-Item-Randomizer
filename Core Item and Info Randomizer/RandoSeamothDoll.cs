using SMLHelper.V2.Assets;
using SMLHelper.V2.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UWE;
using RecipeData = SMLHelper.V2.Crafting.TechData;
using SMLHelper.V2.Crafting;
using DecorationsMod;

namespace CoreItemAndInfoRandomizer
{
	public class RandoSeamothDoll : Craftable
	{
		public RandoSeamothDoll() : base("RandoSeamothDoll", "Randomizer Seamoth Doll", "Randomizer Seamoth Doll")
		{
		}
		public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
		{
			_ = TechTypeHandler.TryGetModdedTechType("RandoSeamothDoll", out TechType outTechType);
			IPrefabRequest task = PrefabDatabase.GetPrefabAsync(CraftData.GetClassIdForTechType(outTechType));
			yield return task;
			_ = task.TryGetPrefab(out GameObject prefab);
			GameObject dollObj = Object.Instantiate(prefab);
			var _RandoSeamothDoll2 = AssetsHelper.Assets.LoadAsset<GameObject>("seamothpuppet");

			dollObj.EnsureComponent<Pickupable>().isPickupable = true;

			gameObject.Set(dollObj);
		}
		protected override RecipeData GetBlueprintRecipe()
		{
			return new RecipeData() { craftAmount = 1, Ingredients = new List<Ingredient>(), LinkedItems = new List<TechType>() };
		}
	}
}
