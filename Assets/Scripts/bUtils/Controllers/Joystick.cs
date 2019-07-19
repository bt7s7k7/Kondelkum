using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace B.Controll {
	public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {
		/*public Vector3 lastPos;
		public bool lastDown;

		private void Update() {
			bool down = IsPressed();
			Vector3 pos = Input.mousePosition;
			if (down) {
				if (!lastDown) {
					lastPos = pos;
				}

				Debug.Log(pos - lastPos);
			}

			lastDown = down;
			lastPos = pos;
		}*/
		[System.Serializable]
		public class VectorEvent : UnityEngine.Events.UnityEvent<Vector3> { }

		[Space]
		public RectTransform center;
		public RectTransform follower;
		public enum AxisType {
			MoveLocal,
			MoveGlobal,
			RotateLocal,
			RotateGlobal
		}
		public AxisType axisType;
		public SinkList target;
		public VectorEvent callback;
		public Vector3 remapX = Vector3.right;
		public Vector3 remapY = Vector3.forward;
		public bool trackpadMode;
		public int down;
		public Vector3 lastPos;

		protected void Reset() {
			center = GetComponent<RectTransform>();
		}

		private void Update() {
			/*if (IsPressed() && !started) {
				started = true;
			}
			bool down = started && Input.GetMouseButton(0);
			if (!down) {
				started = false;
			}

			Vector3 pos = Input.mousePosition;
			Vector3[] corners = new Vector3[4];
			center.GetWorldCorners(corners);
			Vector3 targetCenterPos = (corners[1] + corners[3]) / 2;
			float radius = Mathf.Min(Mathf.Abs(corners[3].x - targetCenterPos.x), Mathf.Abs(corners[3].y - targetCenterPos.y));
			Vector3 clampedPos = Vector3.zero;
			Vector3 output = Vector3.zero;


			if (down) {
				Vector3 realOffset = pos - targetCenterPos;
				float size = realOffset.magnitude;
				if (size > radius) {
					size = radius;
				}
				clampedPos = realOffset.normalized * size;
				output = clampedPos / radius;
				output = output.x * remapX + output.y * remapY;

				if (target) switch (axisType) {
					case AxisType.MoveLocal:
						target.FireMoveLocal(output);
						break;
					case AxisType.MoveGlobal:
						target.FireMoveGlobal(output);
						break;
					case AxisType.RotateLocal:
						target.FireRotateLocal(output);
						break;
					case AxisType.RotateGlobal:
						target.FireRotateGlobal(output);
						break;
					default:
						break;
				}

				callback.Invoke(output);
			}

			if (follower) {
				follower.position = clampedPos + targetCenterPos;
			}*/
			if (down > 0) ProcessPointer(lastPos);
		}

		public void OnPointerDown(PointerEventData eventData) {
			lastPos = eventData.position;
			ProcessPointer(eventData.position);
			down++;
		}

		public void OnPointerUp(PointerEventData eventData) {
			if (follower) {
				Vector3[] corners = new Vector3[4];
				center.GetWorldCorners(corners);
				Vector3 targetCenterPos = (corners[1] + corners[3]) / 2;
				follower.position = targetCenterPos;
			}
			down--;
		}

		public void OnDrag(PointerEventData eventData) {
			ProcessPointer(eventData.position);
		}

		void ProcessPointer(Vector3 ppos) {
			Vector3 pos = ppos;
			Vector3[] corners = new Vector3[4];
			center.GetWorldCorners(corners);
			Vector3 targetCenterPos = (corners[1] + corners[3]) / 2;
			float radius = Mathf.Min(Mathf.Abs(corners[3].x - targetCenterPos.x), Mathf.Abs(corners[3].y - targetCenterPos.y));
			if (trackpadMode) {
				radius = Mathf.Infinity;
			}
			Vector3 clampedPos = Vector3.zero;
			Vector3 output = Vector3.zero;



			Vector3 realOffset = pos - targetCenterPos;
			if (trackpadMode) {
				realOffset = pos - lastPos;
			}
			float size = realOffset.magnitude;
			if (size > radius) {
				size = radius;
			}
			clampedPos = realOffset.normalized * size;
			output = clampedPos / radius;
			if (trackpadMode) {
				output = clampedPos;
			}
			output = output.x * remapX + output.y * remapY;

			if (target) switch (axisType) {
					case AxisType.MoveLocal:
						target.FireMoveLocal(output);
						break;
					case AxisType.MoveGlobal:
						target.FireMoveGlobal(output);
						break;
					case AxisType.RotateLocal:
						target.FireRotateLocal(output);
						break;
					case AxisType.RotateGlobal:
						target.FireRotateGlobal(output);
						break;
					default:
						break;
				}

			callback.Invoke(output);


			if (follower) {
				follower.position = clampedPos + targetCenterPos;
			}

			lastPos = pos;
		}
	}
}