using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearProgressButton : MonoBehaviour {
	public void Act() {
		GameManager.instance.completedLevels.Clear();
		GameManager.instance.SaveLevelCompletion();
		GameManager.instance.LoadMainMenu();
	}
}
