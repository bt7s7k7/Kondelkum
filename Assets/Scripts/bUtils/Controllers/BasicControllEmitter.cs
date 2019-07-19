using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B.Controll {
	public class BasicControllEmitter : AbstractEmitter {
		[System.Serializable]
		public class CustomAction {
			public string name;
			public ControllSetting controllSetting;
		}


		///public BControlls controlls;
		public Vector2 mouseSensitivity = new Vector2(-1, 1);
		public float sensitivityMul = 2.0f;
		[Space]
		public BidirControllSetting vertical = new BidirControllSetting(ControllSetting.Type.Axis, "Vertical");
		public BidirControllSetting horizontal = new BidirControllSetting(ControllSetting.Type.Axis, "Horizontal");

		public BidirControllSetting lookVert = new BidirControllSetting(ControllSetting.Type.Axis, "Mouse X");
		public BidirControllSetting lookHoriz = new BidirControllSetting(ControllSetting.Type.Axis, "Mouse Y");
		[Space]
		public ControllSetting jump = new ControllSetting(ControllSetting.Type.Key, "space");
		public ControllSetting crouch = new ControllSetting(ControllSetting.Type.Key, "left ctrl");
		public ControllSetting interact = new ControllSetting(ControllSetting.Type.Key, "e");
		public ControllSetting fire = new ControllSetting(ControllSetting.Type.Axis, "Fire1");
		public ControllSetting altFire = new ControllSetting(ControllSetting.Type.Axis, "Fire3");
		public ControllSetting scope = new ControllSetting(ControllSetting.Type.Axis, "Fire2");
		public ControllSetting sprint = new ControllSetting(ControllSetting.Type.Key, "left shift");
		public ControllSetting reload = new ControllSetting(ControllSetting.Type.Key, "r");
		public ControllSetting menu = new ControllSetting(ControllSetting.Type.Key, "tab");
		public List<CustomAction> customActions;


		private void Awake() {
			/*Debug.Log("Awaking...");
			controlls.Enable();
			controlls.Default.Move.performed += (ctx) => {
				Vector2 planar = ctx.ReadValue<Vector2>();
				Debug.Log("Moving :" + planar);
				FireMoveLocal(new Vector3(planar.x, 0, planar.y));
			};
			controlls.Default.Look.performed += (ctx) => {
				Vector2 planar = ctx.ReadValue<Vector2>();
				planar = Vector2.Scale(planar, mouseSensitivity) * sensitivityMul;
				FireRotateLocal(new Vector3(planar.x, 0, planar.y));
			};
			controlls.Default.Jump.performed += (ctx) => {
				FireJump();
				FireMoveLocal(Vector3.up);
			};
			controlls.Default.Crouch.performed += (ctx) => {
				FireCrouch();
				FireMoveLocal(Vector3.down);
			};
			controlls.Default.Fire.performed += ctx => FireAttack();
			controlls.Default.AltFire.performed += ctx => FireAltAttack();
			controlls.Default.Interact.performed += ctx => FireInteract();
			controlls.Default.Sprint.performed += ctx => FireSprint();
			controlls.Default.Reload.performed += ctx => FireReload();
			controlls.Default.Scope.performed += ctx => FireScope();
			controlls.Default.Menu.performed += ctx => FireMenu();*/


		}

		private void Start() {
			
		}

		private void Update() {
			target.FireMoveLocal(horizontal.Axis() * Vector3.right + vertical.Axis() * Vector3.forward);
			target.FireRotateLocal((lookHoriz.Axis() * Vector3.right * mouseSensitivity.x + lookVert.Axis() * Vector3.up * mouseSensitivity.y) * sensitivityMul);
			if (jump.Down()) {
				target.FireJump();
			}
			if (crouch.Down()) {
				target.FireCrouch();
			}
			if (fire.Down()) target.FireAttack();
			if (scope.Down()) target.FireScope();
			if (altFire.Down()) target.FireAltAttack();
			if (interact.Down()) target.FireInteract();
			if (sprint.Down()) target.FireSprint();
			if (reload.Down()) target.FireReload();
			if (menu.Down()) target.FireMenu();

			foreach (var v in customActions) {
				if (v.controllSetting.Down()) target.FireAction(v.name);
			}
		}
	}
}