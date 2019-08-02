using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelElements {
	public class Indicator : MonoBehaviour {
		public MeshRenderer[] targetRenderers;
		public Material offMaterial;
		public Material onMaterial;
		public bool state;

		public virtual void SetState(bool newState) {
			state = newState;
			var matToSet = state ? onMaterial : offMaterial;
			foreach (var renderer in targetRenderers) {
				if (!renderer) continue;
				renderer.material = matToSet;
			}
		}

		[B.MethodButton("Activate")]
		public void SetActive() => SetState(true);

		[B.MethodButton("Deactivate")]
		public void SetInactive() => SetState(false);
		public void ToggleState() => SetState(!state);

		[B.MethodButton("Refresh renderers")]
		public void RefreshRenderers() {
			targetRenderers = GetComponentsInChildren<MeshRenderer>();
		}
	}
}