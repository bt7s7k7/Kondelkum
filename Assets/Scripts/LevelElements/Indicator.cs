using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelElements {
	public class Indicator : MonoBehaviour {
		public MeshRenderer targetRenderer;
		public Material offMaterial;
		public Material onMaterial;
		public bool state;

		public virtual void SetState(bool newState) {
			state = newState;
			targetRenderer.material = state ? onMaterial : offMaterial;
		}

		[B.MethodButton("Activate")]
		public void SetActive() => SetState(true);

		[B.MethodButton("Deactivate")]
		public void SetInactive() => SetState(false);
		public void ToggleState() => SetState(!state);
	}
}