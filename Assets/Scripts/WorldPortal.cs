using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPortal : MonoBehaviour {
	private void OnTriggerEnter(Collider other) {
		if (other.tag == GameManager.instance.playerPrefab.tag) {
			other.GetComponent<WorldSwitcher>().Switch();
		}
	}
}
