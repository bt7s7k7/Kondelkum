using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B.Controll {
	public class KeyEventEmitter : MonoBehaviour {
		public ControllSetting controll;
		public UnityEngine.Events.UnityEvent callback;
		public enum Type {
			Down,
			Hold,
			Up
		}
		public Type type;
		protected bool wasDown = false;

		private void Update() {
			bool down = controll.Down();
			if (type == Type.Hold) {
				if (down) callback.Invoke();
			} else if (type == Type.Down) {
				if (down && !wasDown) callback.Invoke();
			} else if (type == Type.Up) {
				if (!down && wasDown) callback.Invoke();
			}

			wasDown = down;
		}
	}
}