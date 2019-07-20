using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour {
	[Header("References")]
	public TextMeshProUGUI label;
	public Button button;
	public TextMeshProUGUI windowLabel;
	public TextMeshProUGUI windowText;
	[Header("Level")]
	[B.RefereceEditor]
	public LevelPrototype levelPrototype;


	[B.MethodButton("Load info")]
	public void Start() {
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

		var buttonColors = button.colors;
		buttonColors.normalColor = levelPrototype.IsCompleted() ? GameManager.instance.completedLevelButtonColor : (levelPrototype.IsUnlocked() ? GameManager.instance.unlockedLevelButtonColor : GameManager.instance.lockedLevelButtonColor);
		button.colors = buttonColors;
	}

	public void OnClick() {
		GameManager.instance.selectedLevel = levelPrototype.scene;
		GameManager.instance.LoadLevel();
	}

}
