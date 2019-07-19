using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace B {
	public class UnityEventTrigger : MonoBehaviour {
		public enum Type {
			None,
			Awake,
			Start,
			Update,
			LateUpdate,
			FixedUpdate,
			OnEnable,
			OnDisable,
			OnDestroy
		}
		public Type type;
		public UnityEvent callback;

		private void Awake() { if (type == Type.Awake) { callback.Invoke(); } }
		private void Start() { if (type == Type.Start) { callback.Invoke(); } }
		private void Update() { if (type == Type.Update) { callback.Invoke(); } }
		private void LateUpdate() { if (type == Type.LateUpdate) { callback.Invoke(); } }
		private void FixedUpdate() { if (type == Type.FixedUpdate) { callback.Invoke(); } }
		private void OnEnable() { if (type == Type.OnEnable) { callback.Invoke(); } }
		private void OnDisable() { if (type == Type.OnDisable) { callback.Invoke(); } }
		private void OnDestroy() { if (type == Type.OnDestroy) { callback.Invoke(); } }

		[B.MethodButton("Run")]
		public void Run() {
			callback.Invoke();
		}
	}
}