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
			bool viable = true;
			if (Physics.Raycast(origin, direction, out RaycastHit hit, toBlock.maxDist, toBlock.raycastMask, QueryTriggerInteraction.Ignore)) {
				pos = hit.point + hit.normal * 0.51f;
				rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
			} else {
				pos = origin + direction * toBlock.maxDist;
			}
			System.Func<bool> checkBox = () => Physics.CheckBox(pos, Vector3.one / 2, rotation, toBlock.raycastMask, QueryTriggerInteraction.Ignore);
			if (checkBox()) {
				if (Physics.Raycast(origin, direction, out hit, 1000, toBlock.raycastMask, QueryTriggerInteraction.Ignore)) {
					pos = hit.point + hit.normal * 0.51f;
					rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
				} else {
					pos = origin + direction * toBlock.maxDist;
				}
				int i = 0;
				for (; i < 5 && checkBox(); i++) {
					pos += hit.normal * 0.1f;
				}
				viable = !checkBox();
			}
			if (viable) {
				Matrix4x4 matrix = Matrix4x4.TRS(pos, rotation, Vector3.one);

				Graphics.DrawMesh(previewMesh, matrix, previewMaterial, 0);

				if (toInteract && !lastDown) {
					gotCube.transform.position = pos;
					gotCube.transform.rotation = rotation;
					var rigidbody = gotCube.GetComponent<Rigidbody>();
					var addCube = gotCube.GetComponent<AddCube>();
					if (rigidbody) {
						rigidbody.velocity = Vector3.zero;
						rigidbody.angularVelocity = Vector3.zero;
					}
					if (addCube.isOneWorld) {
						gotCube.layer = gameObject.layer;
						for (var i = 0; i < gotCube.transform.childCount; i++) {
							gotCube.transform.GetChild(i).gameObject.layer = gameObject.layer;
						}
					}
					gotCube.SetActive(true);
					gotCube = null;


					toBlock.enabled = true;
				}

				if (!toInteract) lastDown = false;
			}
		}

		ResetTasks();
	}

	public void Store(GameObject cube) {
		if (!gotCube) {
			gotCube = cube;
			cube.transform.position = Vector3.up * -1000;
			toBlock.enabled = false;
			lastDown = true;
		}
	}
}
