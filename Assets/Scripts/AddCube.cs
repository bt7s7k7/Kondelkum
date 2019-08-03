using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCube : MonoBehaviour {

	public bool isOneWorld;
	public void TryAddCube(B.Controll.Interacter interacter) {
		interacter.GetComponent<CubeStore>()?.Store(gameObject);
	}
}
