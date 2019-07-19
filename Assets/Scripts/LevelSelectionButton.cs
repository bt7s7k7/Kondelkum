using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour {
	[Header("References")]
	public TextMeshProUGUI label;
	public Image image;
	public TextMeshProUGUI windowLabel;
	public TextMeshProUGUI windowText;
	[Header("Level")]
	public LevelPrototype levelPrototype;


	private void Start() {
		label.text = levelPrototype.number;
		windowLabel.text = levelPrototype.number + (levelPrototype.customLabel.Length > 0 ? ": " + levelPrototype.customLabel : "");
		var desc = "";
		if (levelPrototype.requirements.Length > 0) {
			desc += "Requires: ";
			foreach (var level in levelPrototype.requirements) {
				desc += level.number;
				if (level != levelPrototype.requirements[levelPrototype.requirements.Length - 1]) desc += ", ";
				else desc += "\n";
			}
		}
		desc += levelPrototype.desc;
		windowText.text = desc;
	}

	public void OnClick() {
		GameManager.instance.selectedLevel = levelPrototype.scene;
		GameManager.instance.LoadLevel();
	}

}
