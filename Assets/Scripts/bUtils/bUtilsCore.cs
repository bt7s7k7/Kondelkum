using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace B {
	[System.Serializable]
	public class VectorEvent : UnityEngine.Events.UnityEvent<Vector3> { }
	[System.Serializable]
	public class IntEvent : UnityEngine.Events.UnityEvent<int> { }
	[System.Serializable]
	public class FloatEvent : UnityEngine.Events.UnityEvent<float> { }
	[System.Serializable]
	public class BoolEvent : UnityEngine.Events.UnityEvent<bool> { }
	[System.Serializable]
	public class StringEvent : UnityEngine.Events.UnityEvent<string> { }

	public enum Axis {
		X, Y, Z
	}

	public enum EnumerableType {
		Int,
		Float,
		Bool
	}

	[System.AttributeUsage(System.AttributeTargets.Method)]
	class MethodButtonAttribute : System.Attribute {
		public string name;

		public MethodButtonAttribute(string _name) {
			name = _name;
		}
	}

	[System.Serializable]
	public struct Range {
		public float min;
		public float max;

		public float Get() {
			return Random.Range(min, max);
		}
	}

	[System.Serializable]
	public struct Vector3Range {
		public Vector3 min;
		public Vector3 max;

		public Vector3 Get() {
			return new Vector3(
				Random.Range(min.x, max.x),
				Random.Range(min.y, max.y),
				Random.Range(min.z, max.z)
			);
		}
	}

	public class RefereceEditorAttribute : PropertyAttribute {

	}

	public class BScriptableObject : ScriptableObject {

	}

#if UNITY_EDITOR
	[CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
	[CanEditMultipleObjects]
	public partial class Beditor : Editor {
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			if (serializedObject.isEditingMultipleObjects) return;
			System.Type targetType = target.GetType();
			var methods = targetType.GetMethods();
			foreach (var method in methods) {
				if (method.Name == "GetDebugInfo" && method.ReturnType == typeof(string) && method.GetParameters().Length == 0) {
					EditorGUILayout.HelpBox(method.Invoke(target, new object[0]) as string, MessageType.Info);
				}
			}

			int i = 0;
			EditorGUILayout.BeginHorizontal();
			foreach (var v in methods) {
				if (v.GetParameters().Length == 0) {

					object[] attr = v.GetCustomAttributes(false);
					bool found = false;
					string name = "undefiend";
					foreach (var w in attr) {
						if (w is MethodButtonAttribute button) {
							found = true;
							name = button.name;
							break;
						}
					}
					if (!found) continue;

					if (i % 4 == 3) {
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.BeginHorizontal();
					}
					if (GUILayout.Button(name)) {
						foreach (var w in targets) {
							v.Invoke(v.IsStatic ? null : w, new object[] { });
						}
					}
					i++;
				}
			}
			EditorGUILayout.EndHorizontal();
		}
	}

	[CustomEditor(typeof(ScriptableObject), true, isFallback = true)]
	[CanEditMultipleObjects]
	public class BSOEditor : Beditor {

	}

	static class MenuItems {
		[MenuItem("GameObject/Move to casted position and align")]
		static void MoveToCastAndAlign() {
			if (Selection.activeTransform) {
				Transform active = Selection.activeTransform;
				Camera sceneCamera = SceneView.lastActiveSceneView.camera;

				Ray ray = new Ray(sceneCamera.transform.position, sceneCamera.transform.TransformDirection(Vector3.forward));

				if (Physics.Raycast(ray, out RaycastHit hit)) {
					Undo.RecordObject(active.transform, "Move to casted position and align");
					active.position = hit.point;
					active.position = new Vector3(
						Mathf.Round(active.position.x * 2) / 2,
						Mathf.Round(active.position.y * 2) / 2,
						Mathf.Round(active.position.z * 2) / 2
						);
				}
			}
		}
		[MenuItem("GameObject/Align to grid &#%F")]
		static void AlignToGrid() {
			Transform active = Selection.activeTransform;
			Undo.RecordObject(active.transform, "Align to grid");
			active.position = new Vector3(
									Mathf.Round(active.position.x * 2) / 2,
									Mathf.Round(active.position.y * 2) / 2,
									Mathf.Round(active.position.z * 2) / 2
									);
		}
	}

	/*[CustomEditor(typeof(ValueStore))]
	[CanEditMultipleObjects]
	class ScriptableObjectBeditor : Beditor {
		public override void OnInspectorGUI() {
			Debug.Log("Working");
			base.OnInspectorGUI();
		}
	}*/

	[CustomPropertyDrawer(typeof(Range))]
	class RangeDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(label);
			EditorGUILayout.PropertyField(property.FindPropertyRelative("min"));
			EditorGUILayout.PropertyField(property.FindPropertyRelative("max"));
			EditorGUILayout.EndHorizontal();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUIUtility.singleLineHeight;
		}
	}

	[CustomPropertyDrawer(typeof(RefereceEditorAttribute))]
	class ReferenceEditorAttributeDrawer : PropertyDrawer {
		bool expanded = false;
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			/*if (property.objectReferenceValue != null) {
				object reference = property.objectReferenceValue;
				System.Type type = reference.GetType();
				var props = type.GetProperties();
				foreach (var prop in props) {
					if (prop.Name == "name" && prop.PropertyType == typeof(string)) {
						string rlabel = prop.GetValue(reference) as string;
						label.text += ": " + rlabel;
						break;
					}
				}
				var fields = type.GetFields();
				foreach (var prop in fields) {
					if (prop.Name == "label" && prop.FieldType == typeof(string)) {
						string rlabel = prop.GetValue(reference) as string;
						label.text += ": " + rlabel;
						break;
					}
				}
			}*/
			EditorGUILayout.BeginHorizontal();
			if (property.objectReferenceValue != null) expanded = EditorGUILayout.Foldout(expanded, label);
			EditorGUILayout.PropertyField(property, property.objectReferenceValue != null ? GUIContent.none : label);
			EditorGUILayout.EndHorizontal();

			if (expanded && property.objectReferenceValue != null) {
				EditorGUI.indentLevel++;
				Editor editor = Editor.CreateEditor(property.objectReferenceValue);
				editor.OnInspectorGUI();
				EditorGUI.indentLevel--;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return -2;
		}
	}
#endif
}