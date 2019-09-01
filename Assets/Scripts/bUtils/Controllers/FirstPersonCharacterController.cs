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
			public float maxFallSpeed = 100;
			protected float verticalSpeed;
			public float minY = -100;
			public Vector3 safePos;
			public float gravityMul = 2;
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

				var move = Vector3.zero;

				// Handle jumping
				if (controller.isGrounded && toJump) verticalSpeed += jumpForce;
				// Handle moving
				var delta = Vector3.Scale(toMoveGlobal + body.TransformVector(toMoveLocal), Vector3.forward + Vector3.right);
				var magnitude = delta.magnitude;
				if (magnitude > 1) {
					delta /= magnitude;
				}
				// Add movement delta to character move
				move += delta * Time.deltaTime * speed * (toSprint ? sprintMul : 1) * (toCrouch ? crouchMul : 1);
				// Move based on vertical speed
				move += Vector3.up * verticalSpeed * Time.deltaTime;
#if UNITY_EDITOR
				debug_toMove = toMoveLocal + toMoveGlobal;
				debug_delta = delta;
#endif

				controller.Move(move);

				verticalSpeed += GetGravity() * Time.deltaTime;
				if (verticalSpeed < -maxFallSpeed) verticalSpeed = -maxFallSpeed;

				if (controller.isGrounded) verticalSpeed = -1;

				ResetTasks();

				if (body.position.y < minY) {
					verticalSpeed = 0;
					body.position = safePos;
					Physics.SyncTransforms();
				}
			}

			virtual protected float GetGravity() {
				return Physics.gravity.y * gravityMul;
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
				return "Controller vertical speed: " + verticalSpeed
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