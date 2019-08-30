using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelBoundsCreator : MonoBehaviour {
	public Vector2 levelSize;
	public float height = 100;
	public Transform posX;
	public Transform posZ;
	public Transform negX;
	public Transform negZ;


	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(transform.position, Vector3.one);

		Gizmos.DrawWireCube(transform.position, new Vector3(levelSize.x, 0, levelSize.y));
	}

	[B.MethodButton("Create edges")]
	public void CreateEdges() {
		posX.position = transform.position + Vector3.right * (levelSize.x + 1) / 2;
		posZ.position = transform.position + Vector3.forward * (levelSize.y + 1) / 2;
		negX.position = transform.position - Vector3.right * (levelSize.x + 1) / 2;
		negZ.position = transform.position - Vector3.forward * (levelSize.y + 1) / 2;


		posZ.localScale = negZ.localScale = new Vector3(levelSize.x, height, 1);
		posX.localScale = negX.localScale = new Vector3(levelSize.y, height, 1);
	}

	private void Start() {
		posX.gameObject.SetActive(true);
		posZ.gameObject.SetActive(true);
		negX.gameObject.SetActive(true);
		negZ.gameObject.SetActive(true);
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelBoundsCreator))]
public class LevelBoundsCreatorEditor : B.Beditor {

	bool moving;

	private void OnSceneGUI() {
		var bounds = target as LevelBoundsCreator;
		var size = bounds.levelSize;
		var pos = bounds.transform.position;

		Vector3
			posX = pos + Vector3.right * size.x / 2,
			posZ = pos + Vector3.forward * size.y / 2,
			negX = pos - Vector3.right * size.x / 2,
			negZ = pos - Vector3.forward * size.y / 2
			;

		Vector3 newPosX = Handles.FreeMoveHandle(posX, Quaternion.identity, 0.1f, Vector3.one / 2, Handles.DotHandleCap);
		Vector3 newPosZ = Handles.FreeMoveHandle(posZ, Quaternion.identity, 0.1f, Vector3.one / 2, Handles.DotHandleCap);
		Vector3 newNegX = Handles.FreeMoveHandle(negX, Quaternion.identity, 0.1f, Vector3.one / 2, Handles.DotHandleCap);
		Vector3 newNegZ = Handles.FreeMoveHandle(negZ, Quaternion.identity, 0.1f, Vector3.one / 2, Handles.DotHandleCap);

		Vector3 posDelta = Vector3.zero;
		Vector2 sizeDelta = Vector2.zero;

		float posXDelta = Mathf.Round((newPosX - posX).x);
		float posZDelta = Mathf.Round((newPosZ - posZ).z);
		float negXDelta = -Mathf.Round((newNegX - negX).x);
		float negZDelta = -Mathf.Round((newNegZ - negZ).z);

		posDelta += posXDelta / 4 * Vector3.right;
		sizeDelta += posXDelta / 2 * Vector2.right;
		posDelta -= negXDelta / 4 * Vector3.right;
		sizeDelta += negXDelta / 2 * Vector2.right;

		posDelta += posZDelta / 4 * Vector3.forward;
		sizeDelta += posZDelta / 2 * Vector2.up;
		posDelta -= negZDelta / 4 * Vector3.forward;
		sizeDelta += negZDelta / 2 * Vector2.up;

		if (posDelta.sqrMagnitude > Mathf.Epsilon || sizeDelta.sqrMagnitude > Mathf.Epsilon) {
			if (!moving) {
				moving = true;
				Undo.RecordObjects(new Object[] { bounds, bounds.transform }, "Changed bounds size");
			}
		} else {
			moving = false;
		}

		bounds.levelSize += sizeDelta;
		bounds.transform.position += posDelta;

		if (posDelta.sqrMagnitude > Mathf.Epsilon || sizeDelta.sqrMagnitude > Mathf.Epsilon) {
			PrefabUtility.RecordPrefabInstancePropertyModifications(bounds);
			PrefabUtility.RecordPrefabInstancePropertyModifications(bounds.transform);
		}

		bounds.CreateEdges();
	}
}
#endif