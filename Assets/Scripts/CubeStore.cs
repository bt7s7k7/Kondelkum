using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeStore : B.Controll.AbstractSink {
	public Material previewMaterial;
	public Mesh previewMesh;
	public GameObject gotCube;
	public B.Controll.Interacter toBlock;
	public bool lastDown;

	private void Update() {
		if (gotCube) {
			Vector3 pos;
			Vector3 origin = toBlock.rayOrigin.position;
			Vector3 direction = toBlock.rayOrigin.TransformDirection(Vector3.forward);
			Quaternion rotation = Quaternion.Euler(0, toBlock.rayOrigin.rotation.eulerAngles.y, 0);
			if (Physics.BoxCast(origin, Vector3.one / 2f, direction, out RaycastHit hit, rotation, toBlock.maxDist, toBlock.raycastMask)) {
				pos = hit.point + hit.normal * 0.5f;
				rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
			} else {
				pos = origin + direction * toBlock.maxDist;
			}

			Matrix4x4 matrix = Matrix4x4.TRS(pos, rotation, Vector3.one);

			Graphics.DrawMesh(previewMesh, matrix, previewMaterial, 0);

			if (toInteract && !lastDown) {
				gotCube.transform.position = pos;
				gotCube.transform.rotation = rotation;
				var rigidbody = gotCube.GetComponent<Rigidbody>();
				if (rigidbody) {
					rigidbody.velocity = Vector3.zero;
					rigidbody.angularVelocity = Vector3.zero;
				}
				gotCube.SetActive(true);
				gotCube = null;


				toBlock.enabled = true;
			}

			if (!toInteract) lastDown = false;
		}

		ResetTasks();
	}

	public void Store(GameObject cube) {
		if (!gotCube) {
			gotCube = cube;
			cube.SetActive(false);
			toBlock.enabled = false;
			lastDown = true;
		}
	}
}
