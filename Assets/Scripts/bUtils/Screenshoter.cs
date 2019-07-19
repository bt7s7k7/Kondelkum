using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B {
	public class Screenshoter : MonoBehaviour {
		public Controll.ControllSetting button = new Controll.ControllSetting(Controll.ControllSetting.Type.Key, "f2");
		public int supersize = 1;
		bool wasDown = false;

		[B.MethodButton("Take screenshot")]
		public void TakeScreenshot() {
			ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(Application.persistentDataPath, "screenshots", System.DateTime.Now.ToString("dd_mm_yyyy_hh_mm_ss")) + ".png", supersize);
		}

		private void Update() {
			if (button.Down()) {
				if (!wasDown) {
					TakeScreenshot();
					wasDown = true;
				}
			} else {
				wasDown = false;
			}
		}
	}
}