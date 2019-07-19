using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public enum GameSceneState {
		MainMenu,
		InLevel
	}
	[Header("Level loading")]
	public GameSceneState state;
	public SceneReference selectedLevel;
	public AsyncOperation levelLoadingOperation;
	public GameObject playerPrefab;
	public B.FloatEvent levelLoadingState;
	public UnityEvent levelLoadingStarted;
	public UnityEvent levelLoadingEnded;

	private void Awake() {
		if (!instance) {
			instance = this;
		} else {
			Debug.LogError("A GameManager aleready exists, there can not be >=two");
		}
	}

	private void Start() {
		if (state == GameSceneState.InLevel) {
			LoadLevel();
		}
	}

	public void LoadLevel() {
		if (selectedLevel.ScenePath == "") {
			Debug.LogError("No level selected for loading");
			return;
		}
		Debug.Log("Loading level: " + selectedLevel);
		levelLoadingOperation = SceneManager.LoadSceneAsync(selectedLevel, LoadSceneMode.Additive);
		levelLoadingStarted.Invoke();
	}

	private void Update() {
		if (levelLoadingOperation != null) {
			levelLoadingState.Invoke(levelLoadingOperation.progress);
			if (levelLoadingOperation.isDone) {
				levelLoadingEnded.Invoke();
				levelLoadingOperation = null;
			}
		}
	}
}
