using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace LevelElements {
	public class AreaDetector : MonoBehaviour {
		public int count;
		public B.BoolEvent onStateChanged;
		public B.BoolEvent onStateChangedInv;
		public string[] allowedTags = { "Player", "Cube" };


		private void OnTriggerEnter(Collider other) {
			if (allowedTags.Contains(other.tag)) {
				if (count == 0) {
					onStateChanged.Invoke(true);
					onStateChangedInv.Invoke(false);
				}
				count++;
			}
		}

		private void OnTriggerExit(Collider other) {
			if (allowedTags.Contains(other.tag)) {
				count--;
				if (count == 0) {
					onStateChanged.Invoke(false);
					onStateChangedInv.Invoke(true);
				}
			}
		}
	}
}