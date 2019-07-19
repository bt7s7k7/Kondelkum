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
	public bool wasLevelLoadingOperation;
	public List<AsyncOperation> levelLoadingOperations = new List<AsyncOperation>();
	public GameObject playerPrefab;
	public B.FloatEvent levelLoadingState;
	public UnityEvent levelLoadingStarted;
	public UnityEvent levelLoadingEnded;
	public string levelFolder = "Levels";
	public SceneReference mainMenuScene;

	private void Awake() {
		if (!instance) {
			instance = this;
		} else {
			Debug.LogError("A GameManager aleready exists, there can not be >=two");
		}
	}

	private void Start() {
		
	}

	[B.MethodButton("Load level")]
	public void LoadLevel() {
		UnloadLevelsAndMainMenu();

		if (selectedLevel.ScenePath == "") {
			Debug.LogError("No level selected for loading");
			return;
		}
		Debug.Log("Loading level: " + selectedLevel);
		levelLoadingOperations.Add(SceneManager.LoadSceneAsync(selectedLevel, LoadSceneMode.Additive));
	}

	[B.MethodButton("Unload levels and main menu")]
	public void UnloadLevelsAndMainMenu() {
		var count = SceneManager.sceneCount;
		for (var i = 1; i < count; i++) {
			var scene = SceneManager.GetSceneAt(i);
			if (scene.path.Contains(levelFolder) || scene.path == mainMenuScene) {
				Debug.Log("Unloading level/main menu " + scene.name);
				levelLoadingOperations.Add(SceneManager.UnloadSceneAsync(scene));
			}
		}
	}

	[B.MethodButton("Load main menu")]
	public void LoadMainMenu() {
		UnloadLevelsAndMainMenu();

		if (mainMenuScene.ScenePath == "") {
			Debug.LogError("Main menu scene not set");
			return;
		}
		Debug.Log("Loading main menu");
		levelLoadingOperations.Add(SceneManager.LoadSceneAsync(mainMenuScene, LoadSceneMode.Additive));
	}

	private void Update() {
		HandleAsyncLevelLoadingOperations();
	}

	private void HandleAsyncLevelLoadingOperations() {
		if (levelLoadingOperations.Count > 0) {
			float progress = 0;

			if (!wasLevelLoadingOperation) {
				levelLoadingStarted.Invoke();
			}

			for (int i = 0; i < levelLoadingOperations.Count; i++) {
				progress += levelLoadingOperations[i].progress;
				if (levelLoadingOperations[i].isDone) {
					levelLoadingOperations.RemoveAt(i);
					i--;
				}
			}

			levelLoadingState.Invoke(progress);

			if (levelLoadingOperations.Count == 0) {
				levelLoadingEnded.Invoke();
				wasLevelLoadingOperation = false;
			} else {
				wasLevelLoadingOperation = true;
			}
		}
	}
}
