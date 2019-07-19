using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B.Controll {
	public class Autoregister : MonoBehaviour {
		public SinkList list;
		public bool findListOnStart = true;
		public AbstractSink target;

		private void Reset() {
			list = FindObjectOfType<SinkList>();
			target = GetComponent<AbstractSink>();
		}

		private void Start() {
			if (!list && findListOnStart) list = FindObjectOfType<SinkList>();

			if (list && target) {
				list.AddSink(target);
			}
		}

		private void OnDestroy() {
			if (list && target) {
				list.RemoveSink(target);
			}
		}

		public void SetRegisterEnable(bool enable) {
			enabled = enable;
		}
	}
}