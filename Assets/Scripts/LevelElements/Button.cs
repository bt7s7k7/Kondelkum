using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelElements {
	public class Button : Indicator {
		public B.BoolEvent onStateChange;
		public B.BoolEvent onStateChangeInverted;

		public override void SetState(bool newState) {
			base.SetState(newState);
			onStateChange.Invoke(newState);
			onStateChangeInverted.Invoke(!newState);
		}
	}
}