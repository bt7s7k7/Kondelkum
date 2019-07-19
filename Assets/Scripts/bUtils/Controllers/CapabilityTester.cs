using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B.Controll {
	public class CapabilityTester : MonoBehaviour {
		public AbstractSink.InputCapability requiredCapability;

		public void TestCapability(AbstractSink.InputCapability capability) {
			gameObject.SetActive((capability & requiredCapability) > 0);
		}
	}
}