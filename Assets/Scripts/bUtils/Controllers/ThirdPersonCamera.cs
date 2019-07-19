using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B.Controll {
	public class ThirdPersonCamera : AbstractSink {
		public Transform target;
		public float dist = 5;
		public Vector3 dir;

		private void LateUpdate() {
			dir = dir.normalized;
			dir = Quaternion.AngleAxis(toRotateLocal.y, Vector3.up) * dir;
			dir = Quaternion.AngleAxis(-toRotateLocal.x, Vector3.Cross(dir, Vector3.up)) * dir;
			transform.position = target.TransformPoint(-dir * dist);
			transform.LookAt(target, target.up);

			ResetTasks();
		}

		public override InputCapability GetCapability() {
			return InputCapability.Rotate;
		}
	}
}