using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lerp : MonoBehaviour {

	[System.Serializable]
	public class Action {
		public float frac;
		public UnityEvent action;
		public bool executed;
	}


	public float currentTime;
	public float duration;
	public Transform start;
	public Transform end;
	public Transform target;
	public Action[] actions;
	

	private void Start() {
		currentTime = 0;
	}

	private void Update() {
		currentTime += Time.deltaTime;
		if (currentTime >= duration) {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}

		foreach (var action in actions) {
			if (!action.executed && action.frac <= currentTime / duration) {
				action.executed = true;
				action.action.Invoke();
			}

		}

		target.position = Vector3.Lerp(start.position, end.position, currentTime / duration);
		target.rotation = Quaternion.Lerp(start.rotation, end.rotation, currentTime / duration);
		target.localScale = Vector3.Lerp(start.localScale, end.localScale, currentTime / duration);
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(start.position, end.position);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(start.position, start.position + start.forward);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(end.position, end.position + end.forward);
		Gizmos.color = Color.yellow;
		foreach (var action in actions) {
			Gizmos.DrawWireSphere(Vector3.Lerp(start.position, end.position, action.frac), 0.1f);
		}
	}
}
