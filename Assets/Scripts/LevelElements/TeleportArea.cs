using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sabresaurus.SabreCSG;
using UnityEngine;

namespace LevelElements {
	public class TeleportArea : MonoBehaviour {
		public Transform targetPos;
		public bool addOffset;

		private void OnTriggerEnter(Collider other) {
			if (!targetPos) throw new System.Exception("No target pos specified");
			var newPos = targetPos.position;
			if (addOffset) {
				var offset = other.transform.position - transform.position;
				newPos += offset;
			}
			other.transform.position = newPos;
			Physics.SyncTransforms();
		}
	}
}