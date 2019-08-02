using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B {
	namespace Controll {
		public class FirstPersonCharacterController : AbstractSink {
			public CharacterController controller;
			public Transform head;
			public Transform body;
			public float speed = 10;
			public float sprintMul = 2;
			public float crouchMul = 0.5f;
			public float jumpForce = 5;
			public float fallBoost = 2;
			public float fallBoostThreshold = 0.1f;
			public float maxFallSpeed = 100;
			protected float controllerVerticalSpeed;
			public float minY = -100;
			public Vector3 safePos;
#if UNITY_EDITOR
			protected Vector3 debug_toMove;
			protected Vector3 debug_delta;
			protected Vector3 debug_rotation;
#endif

			virtual protected void Reset() {
				controller = GetComponentInChildren<CharacterController>(true);
				var camera = GetComponentInChildren<Camera>(true);
				if (camera) {
					head = camera.transform;
				}
				body = transform;
			}

			virtual protected void Start() {
				safePos = body.position;
			}

			virtual protected void Update() {

				HandleLooking();

				HandleJumping();
				HandleMovement();
				ResetTasks();

				if (body.position.y < minY) {
					controllerVerticalSpeed = 0;
					body.position = safePos;
				}
			}

			virtual protected void HandleMovement() {
#if UNITY_EDITOR
				debug_toMove = toMoveLocal + toMoveGlobal;
#endif
				var delta = Vector3.Scale(toMoveGlobal + body.TransformVector(toMoveLocal), Vector3.forward + Vector3.right);
				var magnitude = delta.magnitude;
				if (magnitude > 1) {
					delta /= magnitude;
				}
#if UNITY_EDITOR
				debug_delta = delta;
#endif

				controller.Move(delta * Time.deltaTime * speed * (toSprint ? sprintMul : 1) * (toCrouch ? crouchMul : 1));
			}

			virtual protected void HandleJumping() {

				controller.Move(Vector3.up * (controllerVerticalSpeed < fallBoostThreshold ? controllerVerticalSpeed * fallBoost : controllerVerticalSpeed) * Time.deltaTime);
				if (!controller.isGrounded) controllerVerticalSpeed -= -Physics.gravity.y * Time.deltaTime;
				else controllerVerticalSpeed = Physics.gravity.y;
				if (-controllerVerticalSpeed > maxFallSpeed) {
					controllerVerticalSpeed = -maxFallSpeed;
				}

				if (controller.isGrounded && toJump) {
					controllerVerticalSpeed = jumpForce;
				}

			}

			virtual protected void HandleLooking() {
#if UNITY_EDITOR
				debug_rotation = toRotateLocal;
#endif
				body.Rotate(Vector3.up, toRotateLocal.y, Space.World);
				head.Rotate(Vector3.right, toRotateLocal.x, Space.Self);
			}

#if UNITY_EDITOR
			virtual public string GetDebugInfo() {
				return "Controller vertical speed: " + controllerVerticalSpeed
					+ "\nTo move: " + debug_toMove
					+ "\nDelta: " + debug_delta
					+ "\nTo rotate: " + debug_rotation
					;

				;
			}
#endif
		}
	}
}