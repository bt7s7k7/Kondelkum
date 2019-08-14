using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	static public LevelManager instance;
	[B.RefereceEditor]
	public LevelPrototype prototype;

	public void Finish() {
		GameManager.instance.FinishLevel(prototype.id, prototype.nextLevel?.scene);
	}

	private void Awake() {
		instance = this;
	}

}
