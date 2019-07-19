using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace B.Controll {
	public class Button : Selectable {
		public enum Type {
			Interact,
			Attack,
			Jump,
			Crouch,
			Sprint,
			Reload,
			Scope,
			AltAttack,
			Menu,
			Action
		}
		public Type type;
		public string customActionName;
		public SinkList target;

		private void Update() {
			if (IsPressed()) {
				switch (type) {
					case Type.Interact:
						target.FireInteract();
						break;
					case Type.Attack:
						target.FireAttack();
						break;
					case Type.Jump:
						target.FireJump();
						break;
					case Type.Crouch:
						target.FireCrouch();
						break;
					case Type.Sprint:
						target.FireSprint();
						break;
					case Type.Reload:
						target.FireReload();
						break;
					case Type.Scope:
						target.FireScope();
						break;
					case Type.AltAttack:
						target.FireAltAttack();
						break;
					case Type.Menu:
						target.FireMenu();
						break;
					case Type.Action:
						target.FireAction(customActionName);
						break;
					default:
						break;
				}
			}
		}
	}
}