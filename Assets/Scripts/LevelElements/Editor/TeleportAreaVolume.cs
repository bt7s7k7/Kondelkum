using UnityEngine;
using System.Collections;
using System.Linq;
using Sabresaurus.SabreCSG;

namespace LevelElements {
	[System.Serializable]
	public class TeleportAreaVolume : Volume {
		public static Sabresaurus.SabreCSG.Importers.MaterialSearcher searcher = new Sabresaurus.SabreCSG.Importers.MaterialSearcher();
		public Transform targetPos;
		public bool addOffset;

		public override Material BrushPreviewMaterial => searcher.FindMaterial(new string[] { "Collision" });

		public override bool OnInspectorGUI(Volume[] selectedVolumes) {
			var volumes = selectedVolumes.Cast<TeleportAreaVolume>();


			var newTargetPos = UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Target Pos"), targetPos, typeof(Transform), true) as Transform;
			var newAddOffset = UnityEditor.EditorGUILayout.Toggle("Add offset", addOffset);

			if (targetPos != newTargetPos || addOffset != newAddOffset) {

				foreach (var volume in volumes) {
					volume.targetPos = newTargetPos;
					volume.addOffset = newAddOffset;
				}

				return true;
			} else return false;
		}

		public override void OnCreateVolume(GameObject volume) {
			var component = volume.AddComponent<TeleportArea>();

			component.addOffset = addOffset;
			component.targetPos = targetPos;
		}
	}
}