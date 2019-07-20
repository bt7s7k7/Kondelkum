using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelButtonInfo : MonoBehaviour {
	public TextMeshProUGUI label;
	public TextMeshProUGUI desc;
	public static LevelButtonInfo instance;

	private void Awake() {
		instance = this;
	}
}
