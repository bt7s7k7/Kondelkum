using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace B.Controll {
	public class Interacter : AbstractSink {
		[System.Serializable]
		public class InteractEvent : UnityEvent<Interacter> { }

		public LayerMask raycastMask;
		public float maxDist;
		public bool once = true;
		public Transform rayOrigin;

		private void Reset() {
			rayOrigin = GetComponentInChildren<Camera>()?.transform;
		}

		bool lastShould;

		private void Update() {
			if (toInteract && (once ? !lastShould : true)) {
				var ray = new Ray(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward));

				if (Physics.Raycast(ray, out RaycastHit hit, maxDist, raycastMask)) {
					Interactable hitInteractable = hit.collider.GetComponentInParent<Interactable>();	
					if (hitInteractable) {
						hitInteractable.Interact(this);
					}
				}
			}
			lastShould = toInteract;

			ResetTasks();
		}

	}
};