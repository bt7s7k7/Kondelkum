using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B {
	public class Array : MonoBehaviour {
		public enum PositionType {
			Even,
			Random
		}
		public PositionType positionType;
		public Vector3 bounds;
		public Vector3Int amount;
		public Vector3Range scaleRange = new Vector3Range { min = Vector3.one, max = Vector3.one };
		public bool randomizeRotation;
		public bool onStart;
		public bool clearChildren = true;
		public GameObject[] prefabs;

		private void Start() {
			if (onStart) {
				Generate();
			}
		}

		private void OnDrawGizmosSelected() {
			Gizmos.color = Color.green;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, bounds);
			Gizmos.matrix = Matrix4x4.identity;
		}

		[B.MethodButton("Generate")]
		public void Generate() {
			if (clearChildren) {
				Transform[] children = new Transform[transform.childCount];
				{
					int i = 0;
					foreach (Transform child in transform) {
						children[i] = child;
						i++;
					}
				}

				foreach (Transform child in children) {
					DestroyImmediate(child.gameObject);
				}

				if (positionType == PositionType.Even) {
					Vector3 scale = new Vector3(
						bounds.x / (amount.x - 1),
						bounds.y / (amount.y - 1),
						bounds.z / (amount.z - 1)
					);
					for (int x = 0; x < amount.x; x++) {
						for (int y = 0; y < amount.y; y++) {
							for (int z = 0; z < amount.z; z++) {
								Place(-bounds / 2 + Vector3.Scale(new Vector3(x, y, z), scale));
							}
						}
					}
				} else if (positionType == PositionType.Random) {
					for (int i = 0; i < amount.x; i++) {
						Vector3 pos = new Vector3(
							Random.Range(-bounds.x / 2, bounds.x / 2),
							Random.Range(-bounds.y / 2, bounds.y / 2),
							Random.Range(-bounds.z / 2, bounds.z / 2)
						);
						Place(pos);
					}
				}
			}
		}

		void Place(Vector3 pos) {
			GameObject spawned = Instantiate(prefabs[Mathf.FloorToInt(Random.Range(0, prefabs.Length - 1))], transform.TransformPoint(pos), randomizeRotation ? Random.rotation : Quaternion.identity, transform);
			spawned.transform.localScale = new Vector3(
				Random.Range(scaleRange.min.x, scaleRange.max.x),	
				Random.Range(scaleRange.min.y, scaleRange.max.y),	
				Random.Range(scaleRange.min.z, scaleRange.max.z)	
			);
			spawned.name += " " + pos;
		}
	}
}
