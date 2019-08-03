using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New level", menuName ="Level", order =100000)]
public class LevelPrototype : ScriptableObject {
	public string customLabel;
	public string number;
	public string id;
	public LevelPrototype[] requirements;
	public SceneReference scene;
	public bool allowWorldSwitch;
	[Multiline]
	public string desc;

	public bool IsCompleted() {
		return GameManager.instance.IsLevelCompleted(id);
	}

	public bool IsUnlocked() {
		bool ret = true;
		foreach (var level in requirements) {
			if (!level.IsCompleted()) {
				ret = false;
				break;
			}
		}
		return ret;
	}
}
