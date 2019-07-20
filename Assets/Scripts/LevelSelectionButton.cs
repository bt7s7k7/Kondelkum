using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour {
	[Header("References")]
	public TextMeshProUGUI label;
	public Button button;
	[Header("Level")]
	[B.RefereceEditor]
	public LevelPrototype levelPrototype;


	[B.MethodButton("Load info")]
	public void Start() {
		label.text = levelPrototype.number;
		

		var buttonColors = button.colors;
		buttonColors.normalColor = levelPrototype.IsCompleted() ? GameManager.instance.completedLevelButtonColor : (levelPrototype.IsUnlocked() ? GameManager.instance.unlockedLevelButtonColor : GameManager.instance.lockedLevelButtonColor);
		buttonColors.disabledColor = buttonColors.normalColor;
		button.colors = buttonColors;

		button.interactable = levelPrototype.IsUnlocked();
	}

	public void OnHover() {
		LevelButtonInfo.instance.label.text = levelPrototype.number + (levelPrototype.customLabel.Length > 0 ? ": " + levelPrototype.customLabel : "");
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
		LevelButtonInfo.instance.desc.text = desc;
	}

	public void OnClick() {
		if (levelPrototype.IsUnlocked()) {
			GameManager.instance.selectedLevel = levelPrototype.scene;
			GameManager.instance.LoadLevel();
		}
	}

}
