using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace B {
	namespace Controll {
		
		public class AbstractEmitter : MonoBehaviour {
			public SinkList target;

			protected virtual void Reset() {
				target = GetComponent<SinkList>();
			}
		}

		[System.Serializable]
		public class ControllSetting {

			public static bool IsKeyValid(string name) {
				try {
					Input.GetKey(name);
					return true;
				} catch (System.Exception) {
					return false;
				}
			}

			public static bool IsAxisValid(string name) {
				try {
					Input.GetButton(name);
					return true;
				} catch (System.Exception) {
					return false;
				}
			}

			public enum Type {
				Key,
				Axis
			};
			public Type type;
			public string name;

			public bool Down() {
				if (type == Type.Axis) {
					if (!IsAxisValid(name)) return false;
					return Input.GetButton(name);
				} else {
					if (!IsKeyValid(name)) return false;
					return Input.GetKey(name);
				}
			}

			public ControllSetting(Type t, string n) {
				type = t;
				name = n;
			}
		}
		[System.Serializable]
		public class BidirControllSetting : ControllSetting {
			public string negativeName;

			public float Axis() {
				if (type == Type.Axis) {
					if (!IsAxisValid(name)) return 0;
					return Input.GetAxis(name);
				} else {
					float ret = 0;

					if (IsKeyValid(name) && Input.GetKey(name)) ret += 1;
					if (IsKeyValid(negativeName) && Input.GetKey(negativeName)) ret -= 1;

					return ret;
				}

			}

			public BidirControllSetting(Type t, string n, string neg = "") : base(t, n) {
				negativeName = neg;
			}
		}
#if UNITY_EDITOR
		[CustomPropertyDrawer(typeof(ControllSetting))]
		class ControllSettingDrawer : PropertyDrawer {

			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel(label);
				MakeFields(property);
				EditorGUILayout.EndHorizontal();

			}



			protected virtual void MakeFields(SerializedProperty property) {
				SerializedProperty type = property.FindPropertyRelative("type");
				Color color = GUI.color;
				bool isKey = type.enumValueIndex == (int)ControllSetting.Type.Key;
				if (isKey) {
					GUI.color = new Color(1, 1, 0.5f, 1);
				} else {
					GUI.color = new Color(0.5f, 1, 0.5f, 1);
				}
				EditorGUILayout.PropertyField(type, GUIContent.none, GUILayout.MaxWidth(50));

				SerializedProperty name = property.FindPropertyRelative("name");
				bool valid = true;
				if (isKey) {
					valid = ControllSetting.IsKeyValid(name.stringValue);
				} else {
					valid = ControllSetting.IsAxisValid(name.stringValue);
				}
				if (valid) {
					GUI.color = new Color(1, 1, 1, 1);
				} else {
					GUI.color = new Color(1, 0.5f, 0.5f, 1);
				}
				EditorGUILayout.PropertyField(name, GUIContent.none);
				GUI.color = color;
			}

			public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
				return 0; //EditorGUIUtility.singleLineHeight/16;
			}
		}
		[CustomPropertyDrawer(typeof(BidirControllSetting))]
		class BidirControllSettingDrawer : ControllSettingDrawer {
			protected override void MakeFields(SerializedProperty property) {
				base.MakeFields(property);
				if ((ControllSetting.Type)property.FindPropertyRelative("type").enumValueIndex == ControllSetting.Type.Key) {
					Color color = GUI.color;
					SerializedProperty altName = property.FindPropertyRelative("negativeName");
					if (ControllSetting.IsKeyValid(altName.stringValue)) {
						GUI.color = new Color(1, 1, 1, 1);
					} else {
						GUI.color = new Color(1, 0.5f, 0.5f, 1);
					}
					EditorGUILayout.PropertyField(altName, GUIContent.none);
					GUI.color = color;
				}
			}
		}
#endif

	}
}
