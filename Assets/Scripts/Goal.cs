﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		if (other.tag == GameManager.instance.playerPrefab.tag) {
			LevelManager.instance.Finish();
		}
	}
}
