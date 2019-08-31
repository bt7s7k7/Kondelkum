using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDDot : MonoBehaviour {
	public UnityEngine.UI.Image target;

	void Update() {
		target.enabled = LevelManager.instance != null;
	}
}
