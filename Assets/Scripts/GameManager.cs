using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

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
	public int loadedLevelID;

	[Header("Constants")]
	public Color lockedLevelButtonColor;
	public Color unlockedLevelButtonColor;
	public Color completedLevelButtonColor;

	[Header("Level progress")]
	public List<string> completedLevels;


	private void Awake() {
		if (!instance) {
			instance = this;
		} else {
			Debug.LogError("A GameManager aleready exists, there can not be >=two");
		}

		completedLevels = PlayerPrefs.GetString("GameManager_completedLevels", "").Split(',').ToList();
		if (completedLevels.Count == 1 && completedLevels[0] == "") completedLevels.Clear();
	}

	private void OnDestroy() {
		SaveLevelCompletion();
	}

	[B.MethodButton("Save level completion")]
	public void SaveLevelCompletion() {
		PlayerPrefs.SetString("GameManager_completedLevels", String.Join(",", completedLevels));
	}

	public bool IsLevelCompleted(string number) {
		return completedLevels.Contains(number);
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
		loadedLevelID = SceneUtility.GetBuildIndexByScenePath(selectedLevel);

		state = GameSceneState.InLevel;
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

		state = GameSceneState.MainMenu;
	}

	public void FinishLevel(string number) {
		LoadMainMenu();
		if (!completedLevels.Contains(number)) completedLevels.Add(number);
		SaveLevelCompletion();
	}

	public void SpawnPlayer(Vector3 pos, Transform parent) {
		var player = Instantiate(playerPrefab, pos, Quaternion.identity, parent);
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
