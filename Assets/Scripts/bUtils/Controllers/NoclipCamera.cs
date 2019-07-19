using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B.Controll {
	public class NoclipCamera : AbstractSink {
		public float speed = 10;
		public float sprintMul = 5;
		public Transform target;

		private void Reset() {
			target = transform;
		}

		private void Update() {
			float currSpeed = speed * Time.deltaTime;
			if (toSprint) currSpeed *= sprintMul;
			target.Translate(Vector3.Scale(toMoveLocal, Vector3.forward + Vector3.right) * currSpeed, Space.Self);
			target.Translate((Vector3.Scale(toMoveLocal, Vector3.up) + toMoveGlobal) * currSpeed + (toJump ? Vector3.up * currSpeed : Vector3.zero) + (toCrouch ? Vector3.down * currSpeed : Vector3.zero), Space.World);

			target.Rotate(Vector3.Scale(toRotateLocal, Vector3.right), Space.Self);
			target.Rotate(toRotateGlobal + Vector3.Scale(toRotateLocal, Vector3.up), Space.World);

			ResetTasks();
		}

		public override InputCapability GetCapability() {
			return InputCapability.Noclip;

		}

		public override void RotateAt(Vector3 vector) {
			target.LookAt(vector, Vector3.up);
		}
	}
}