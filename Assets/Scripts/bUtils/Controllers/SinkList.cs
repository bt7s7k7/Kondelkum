using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B.Controll {
	public class SinkList : MonoBehaviour {
		public List<AbstractSink> _sinks;
		public CapabilityEvent capabilitySet;

		[System.Serializable]
		public class CapabilityEvent : UnityEngine.Events.UnityEvent<AbstractSink.InputCapability> {}

		public virtual void FireMoveLocal(Vector3 vector) {
			foreach (var v in _sinks) {
				v.MoveLocal(vector);
			}
		}
		public virtual void FireMoveGlobal(Vector3 vector) {
			foreach (var v in _sinks) {
				v.MoveGlobal(vector);
			}
		}
		public virtual void FireRotateLocal(Vector3 vector) {
			foreach (var v in _sinks) {
				v.RotateLocal(vector);
			}
		}
		public virtual void FireRotateGlobal(Vector3 vector) {
			foreach (var v in _sinks) {
				v.RotateGlobal(vector);
			}
		}
		public virtual void FireRotateAt(Vector3 vector) {
			foreach (var v in _sinks) {
				v.RotateAt(vector);
			}
		}
		public virtual void FireInteract() {
			foreach (var v in _sinks) {
				v.Interact();
			}
		}
		public virtual void FireAttack() {
			foreach (var v in _sinks) {
				v.Attack();
			}
		}
		public virtual void FireJump() {
			foreach (var v in _sinks) {
				v.Jump();
			}
		}
		public virtual void FireSprint() {
			foreach (var v in _sinks) {
				v.Sprint();
			}
		}
		public virtual void FireCrouch() {
			foreach (var v in _sinks) {
				v.Crouch();
			}
		}
		public virtual void FireReload() {
			foreach (var v in _sinks) {
				v.Reload();
			}
		}
		public virtual void FireScope() {
			foreach (var v in _sinks) {
				v.Scope();
			}
		}
		public virtual void FireMenu() {
			foreach (var v in _sinks) {
				v.Menu();
			}
		}
		public virtual void FireAltAttack() {
			foreach (var v in _sinks) {
				v.AltAttack();
			}
		}
		public virtual void FireAction(string name) {
			foreach (var v in _sinks) {
				v.Action(name);
			}
		}

		public AbstractSink.InputCapability CollectCapability() {
			AbstractSink.InputCapability ret = AbstractSink.InputCapability.None;
			foreach (var v in _sinks) {
				if (v.enabled && v.gameObject.activeInHierarchy) ret |= v.GetCapability();
			}
			return ret;
		}

		[MethodButton("Refresh capabilities")]
		public void RefreshCapabilities() {
			capabilitySet.Invoke(CollectCapability());
		}

		public void AddSink(AbstractSink sink) {
			if (!_sinks.Contains(sink)) {
				_sinks.Add(sink);
				RefreshCapabilities();
			}
		}

		public void RemoveSink(AbstractSink sink) {
			if (_sinks.Contains(sink)) {
				_sinks.Remove(sink);
				RefreshCapabilities();
			}
		}
	}
}