using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sabresaurus.SabreCSG;
using System.Linq;
using System.Globalization;

public class GameObjectVolume : Volume {

	public static GameObject[] taggedObjects = null;
	public GameObject selected;

	public override Material BrushPreviewMaterial => SabreCSGResources.GetCircleMaterial(); 

	public void Refresh() {
		taggedObjects = AssetDatabase.FindAssets("l:BlockVolume")
			.Select(v => AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(v), typeof(GameObject)))
			.Cast<GameObject>()
			.ToArray();
	}

	public override bool OnInspectorGUI(Volume[] selectedVolumes) {
		bool dirty = false;
		var volumes = selectedVolumes.Cast<GameObjectVolume>();

		if (GUILayout.Button("Refresh") || taggedObjects == null) {
			Refresh();
			dirty = true;
		}


		var index = 0;

		if (selected != null) index = System.Array.IndexOf(taggedObjects, selected);
		else dirty = true;

		if (index == -1) {
			index = 0;
			dirty = true;
		}

		var newIndex = EditorGUILayout.Popup("Prefab", index, taggedObjects.Select(v => v.name).ToArray());

		if (newIndex != index) dirty = true;

		if (dirty) {
			foreach (var volume in volumes) {
				volume.selected = taggedObjects[newIndex];
			}
		}

		return dirty;
	}

	public override void OnCreateVolume(GameObject volume) {
		var instance = Instantiate(selected, volume.transform, false).transform;
		instance.transform.localPosition = Vector3.zero;
		instance.transform.localRotation = Quaternion.identity;
		string source = volume.transform.parent.gameObject.name;
		var start = source.LastIndexOf("(");
		var dimensions = source.Substring(start + 1, source.Length - start - 2).Split('x').Select(v => float.Parse(v, CultureInfo.InvariantCulture)).ToArray();

		instance.transform.localScale = new Vector3(dimensions[0], dimensions[1], dimensions[2]);
	}
}
