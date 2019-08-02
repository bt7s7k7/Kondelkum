using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B.Controll {
	public class Interactable : MonoBehaviour {
		public Interacter.InteractEvent onInteracted;

		public void Interact(Interacter interacter) => onInteracted.Invoke(interacter);
	}
}