using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B.Controll {
	public class AnimatorParameterController : AbstractSink {
		public Animator target;
		public string valueName;
		public EnumerableType valueType;
		public bool isAxis;
		public Button.Type actionType;
		public string actionName;
		public Joystick.AxisType axisType;
		public Axis axis;
		public float mul = 1;
		private float lastSetValue;

		private void Reset() {
			target = GetComponent<Animator>();
		}

		private void Update() {
			float value = 0;

			if (isAxis) {
				Vector3 axisValue = Vector3.zero;
				switch (axisType) {
					case Joystick.AxisType.MoveLocal:
						axisValue = toMoveLocal;
						break;
					case Joystick.AxisType.MoveGlobal:
						axisValue = toMoveGlobal;
						break;
					case Joystick.AxisType.RotateLocal:
						axisValue = toRotateLocal;
						break;
					case Joystick.AxisType.RotateGlobal:
						axisValue = toRotateGlobal;
						break;
				}
				switch (axis) {
					case Axis.X:
						value = axisValue.x;
						break;
					case Axis.Y:
						value = axisValue.y;
						break;
					case Axis.Z:
						value = axisValue.z;
						break;
				}
			} else {
				switch (actionType) {
					case Button.Type.Interact:
						value = toInteract ? 1 : 0;
						break;
					case Button.Type.Attack:
						value = toAttack ? 1 : 0;
						break;
					case Button.Type.Jump:
						value = toJump ? 1 : 0;
						break;
					case Button.Type.Crouch:
						value = toCrouch ? 1 : 0;
						break;
					case Button.Type.Sprint:
						value = toSprint ? 1 : 0;
						break;
					case Button.Type.Reload:
						value = toReload ? 1 : 0;
						break;
					case Button.Type.Scope:
						value = toScope ? 1 : 0;
						break;
					case Button.Type.AltAttack:
						value = toAltAttack ? 1 : 0;
						break;
					case Button.Type.Menu:
						value = toMenu ? 1 : 0;
						break;
				}
			}

			value *= mul;
			lastSetValue = value;

			switch (valueType) {
				case EnumerableType.Int:
					target.SetInteger(valueName, Mathf.FloorToInt(value));
					break;
				case EnumerableType.Float:
					target.SetFloat(valueName, value);
					break;
				case EnumerableType.Bool:
					target.SetBool(valueName, value > 0);
					break;
			}

			ResetTasks();
		}

		public string GetDebugInfo() {
			if (target) {
				var parameters = target.parameters;
				string[] lines = new string[parameters.Length];
				for (var i = 0; i < lines.Length; i++) {
					lines[i] = parameters[i].name + " : " + parameters[i].type.ToString();
				}
				return string.Join("\n", lines) + "\n Last value: " + lastSetValue;
			} else return "No target";
		}

	}
}
