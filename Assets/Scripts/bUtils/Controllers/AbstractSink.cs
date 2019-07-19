using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B {
	namespace Controll {
		public class AbstractSink : MonoBehaviour {
			public virtual void MoveLocal(Vector3 vector) { toMoveLocal = toMoveLocal + vector; }
			public virtual void MoveGlobal(Vector3 vector) { toMoveGlobal = toMoveGlobal + vector; }
			public virtual void RotateLocal(Vector3 vector) { toRotateLocal = toRotateLocal + vector; }
			public virtual void RotateGlobal(Vector3 vector) { toRotateGlobal = toRotateGlobal + vector; }
			public virtual void RotateAt(Vector3 vector) { }

			protected Vector3 toMoveLocal;
			protected Vector3 toMoveGlobal;
			protected Vector3 toRotateLocal;
			protected Vector3 toRotateGlobal;

			public virtual void Interact() { toInteract = true; }
			public virtual void Attack() { toAttack = true; }
			public virtual void Jump() { toJump = true; }
			public virtual void Crouch() { toCrouch = true; }
			public virtual void Sprint() { toSprint = true; }
			public virtual void Reload() { toReload = true; }
			public virtual void Scope() { toScope = true; }
			public virtual void AltAttack() { toAltAttack = true; }
			public virtual void Menu() { toMenu = true; }
			public virtual void Action(string name) { }

			public enum InputCapability {
				None = 0,
				Move = 1 << 0,
				Rotate = 1 << 1,
				Noclip = Move | Rotate,
				ComplexMove = Move | 1 << 2,
				Walk = ComplexMove | Rotate,
				Shoot = 1 << 3,
				Scope = 1 << 4,
				FPS = Shoot | Scope,
				Interact = 1 << 5,
				Menu = 1 << 6,
				Custom1 = 1 << 7,
				Custom2 = 1 << 8,
				Custom3 = 1 << 9,
				Custom4 = 1 << 10
			}
			public virtual InputCapability GetCapability() {
				return InputCapability.None;
			}

			protected bool toInteract;
			protected bool toAttack;
			protected bool toJump;
			protected bool toCrouch;
			protected bool toSprint;
			protected bool toReload;
			protected bool toScope;
			protected bool toAltAttack;
			protected bool toMenu;

			protected void ResetTasks() {
				toInteract = false;
				toAttack = false;
				toJump = false;
				toCrouch = false;
				toSprint = false;
				toReload = false;
				toScope = false;
				toAltAttack = false;
				toMenu = false;

				toMoveLocal = Vector3.zero;
				toMoveGlobal = Vector3.zero;
				toRotateLocal = Vector3.zero;
				toRotateGlobal = Vector3.zero;
			}

			protected void OnEnable() {
				ResetTasks();
			}

		}
	}
}