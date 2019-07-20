using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	private void Start() {
		GameManager.instance.SpawnPlayer(transform.position);
	}
}
