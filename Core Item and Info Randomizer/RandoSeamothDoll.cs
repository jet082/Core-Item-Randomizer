using SMLHelper.V2.Assets;
using SMLHelper.V2.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UWE;
using RecipeData = SMLHelper.V2.Crafting.TechData;
using SMLHelper.V2.Crafting;

namespace CoreItemAndInfoRandomizer
{
	public class RandoSeamothDoll : Craftable
	{
		public RandoSeamothDoll() : base("RandoCyclopsDoll", "Cyclops", "Cyclops")
		{
		}
		public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
		{
			_ = TechTypeHandler.TryGetModdedTechType("CyclopsDoll", out TechType outTechType);
			IPrefabRequest task = PrefabDatabase.GetPrefabAsync(CraftData.GetClassIdForTechType(outTechType));
			yield return task;
			_ = task.TryGetPrefab(out GameObject prefab);
			GameObject dollObj = Object.Instantiate(prefab);

			Pickupable pickupable = dollObj.AddComponent<Pickupable>();
			pickupable.enabled = true;

			gameObject.Set(dollObj);
		}
		protected override RecipeData GetBlueprintRecipe()
		{
			return new RecipeData() { craftAmount = 1, Ingredients = new List<Ingredient>(), LinkedItems = new List<TechType>() };
		}
	}
}
