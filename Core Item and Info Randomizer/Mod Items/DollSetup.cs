using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoreItemAndInfoRandomizer
{
	public class DollSetup
	{
		public static HashSet<Type> AllowList = new() { typeof(Rigidbody), typeof(Transform), typeof(LargeWorldEntity), typeof(TechTag), typeof(PrefabIdentifier), typeof(WorldForces), typeof(Pickupable), typeof(SkyApplier) };
		public static GameObject SetupDoll(GameObject prefab, float scaler)
		{
			GameObject dollObj = GameObject.Instantiate(prefab);
			bool stillHasComponents = true;
			while (stillHasComponents)
			{
				stillHasComponents = false;
				foreach (var someComponent in dollObj.GetComponents<Component>())
				{
					if (!AllowList.Contains(someComponent.GetType()))
					{
						MainLogicLoop.DebugWrite($"Let's look at {someComponent.GetType()}");
						GameObject.DestroyImmediate(someComponent);
						stillHasComponents = true;
					}
				}
			}
			Pickupable pickupable = dollObj.EnsureComponent<Pickupable>();
			pickupable.isPickupable = true;
			pickupable.enabled = true;
			dollObj.transform.localScale = new Vector3(scaler, scaler, scaler);
			Rigidbody dollBody = dollObj.EnsureComponent<Rigidbody>();
			dollBody.isKinematic = true;
			dollBody.useGravity = true;

			return dollObj;
		}
	}
}
