using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelElements {
	public class Button : Indicator {
		public B.BoolEvent onStateChange;
		public B.BoolEvent onStateChangeInverted;
		public bool doReset;

		public override void SetState(bool newState) {
			base.SetState(newState);
			onStateChange.Invoke(newState);
			onStateChangeInverted.Invoke(!newState);

			if (doReset) {
				Invoke("SetFalseState", 0.01f);
			}
		}

		public void SetFalseState() => SetState(false);
	}
}