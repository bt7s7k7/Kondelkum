using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New level", menuName = "Level", order = 100000)]
public class LevelPrototype : ScriptableObject {
	public string customLabel;
	public string number;
	public string id;
	[B.RefereceEditor]
	public LevelPrototype[] requirements;
	public SceneReference scene;
	public bool allowWorldSwitch;
	public bool allowSwitchWithCube;
	[B.RefereceEditor]
	public LevelPrototype nextLevel;
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

#if UNITY_EDITOR
	[B.MethodButton("Init scene")]
	public void InitScene() {
		var createdScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
		if (!EditorSceneManager.SaveScene(createdScene)) {
			EditorSceneManager.UnloadSceneAsync(createdScene);
			return;
		}
		EditorSceneManager.SetActiveScene(createdScene);

		GameObject levelManagerObject = new GameObject("LevelManager");
		var levelManager = levelManagerObject.AddComponent<LevelManager>();
		levelManager.prototype = this;

		scene.ScenePath = createdScene.path;
	}
#endif
}
