using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B.Controll {
	public class MouseLocker : MonoBehaviour {
		public bool lockMouse;
		public bool hideMouse;

		private void Update() {
			Cursor.lockState = lockMouse ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = !hideMouse;
		}
	}
}